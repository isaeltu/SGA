using System.Security.Cryptography;
using MediatR;
using SGA.Application.Commands;
using SGA.Application.Rules;
using SGA.Domain.Entities.Authorizations;
using SGA.Domain.Entities.Users;
using SGA.Domain.Enums.Trips;
using SGA.Domain.Repositories;
using SGA.Domain.Repositories.Users;
using SGA.Domain.ValueObjects.Users;

namespace SGA.Application.Handlers
{
    public sealed class CreateGuestReservationHandler : IRequestHandler<CreateGuestReservationCommand, int>
    {
        private readonly ITripRepository _tripRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISender _sender;

        public CreateGuestReservationHandler(
            ITripRepository tripRepository,
            IPersonRepository personRepository,
            IStudentRepository studentRepository,
            IAuthorizationRepository authorizationRepository,
            IRoleRepository roleRepository,
            ISender sender)
        {
            _tripRepository = tripRepository;
            _personRepository = personRepository;
            _studentRepository = studentRepository;
            _authorizationRepository = authorizationRepository;
            _roleRepository = roleRepository;
            _sender = sender;
        }

        public async Task<int> Handle(CreateGuestReservationCommand request, CancellationToken cancellationToken)
        {
            var trip = await _tripRepository.GetByIdAsync(request.TripId, cancellationToken).ConfigureAwait(false);
            if (trip is null)
            {
                throw new KeyNotFoundException($"Trip with id {request.TripId} was not found.");
            }

            var utcNow = DateTime.UtcNow;
            if (TripBookingRules.MustAutoCancel(trip, utcNow))
            {
                var cancelResult = trip.Cancel("system-auto-cancel");
                if (cancelResult.IsSuccess)
                {
                    await _tripRepository.UpdateAsync(trip, cancellationToken).ConfigureAwait(false);
                }
            }

            if (TripBookingRules.HasDeparturePassed(trip, utcNow))
            {
                throw new InvalidOperationException("Reservations are not allowed after the trip departure time.");
            }

            if (trip.InstitutionId != request.InstitutionId)
            {
                throw new InvalidOperationException("The selected trip does not belong to the selected institution.");
            }

            if (trip.Status != TripStatus.Scheduled && trip.Status != TripStatus.InProgress)
            {
                throw new InvalidOperationException("Reservations are only allowed for scheduled or in-progress trips.");
            }

            var roleId = await EnsureGuestRoleAsync(request.CreatedBy, cancellationToken).ConfigureAwait(false);
            var person = await EnsurePersonAsync(request, roleId, cancellationToken).ConfigureAwait(false);
            var student = await EnsureStudentAsync(person.Id, request.CreatedBy, cancellationToken).ConfigureAwait(false);
            var authorization = await EnsureAuthorizationAsync(student.Id, request.CreatedBy, cancellationToken).ConfigureAwait(false);

            return await _sender.Send(
                new CreateReservationCommand(request.TripId, person.Id, authorization.Id, request.CreatedBy),
                cancellationToken).ConfigureAwait(false);
        }

        private async Task<int> EnsureGuestRoleAsync(string createdBy, CancellationToken cancellationToken)
        {
            var existingRole = await _roleRepository.GetByNameAsync("Cliente", cancellationToken).ConfigureAwait(false);
            if (existingRole is not null)
            {
                return existingRole.Id;
            }

            var role = new Role("Cliente", "Rol para reservas publicas de la web", createdBy);
            await _roleRepository.AddAsync(role, cancellationToken).ConfigureAwait(false);
            return role.Id;
        }

        private async Task<Person> EnsurePersonAsync(CreateGuestReservationCommand request, int roleId, CancellationToken cancellationToken)
        {
            var normalizedEmail = NormalizeOrNull(request.Email);
            if (normalizedEmail is not null)
            {
                var existingByEmail = await _personRepository.GetByEmailAsync(normalizedEmail, cancellationToken).ConfigureAwait(false);
                if (existingByEmail is not null)
                {
                    return existingByEmail;
                }
            }

            var firstName = NormalizeName(request.FirstName, "Invitado");
            var lastName = NormalizeName(request.LastName, "Web");
            var email = normalizedEmail ?? GenerateGuestEmail();
            var phoneNumber = NormalizePhoneOrDefault(request.PhoneNumber);
            var cedula = await GenerateUniqueGuestCedulaAsync(cancellationToken).ConfigureAwait(false);

            var person = new Person(
                roleId,
                email,
                phoneNumber,
                firstName,
                lastName,
                cedula)
            {
                InstitutionId = request.InstitutionId
            };

            person.SetCreationInfo(request.CreatedBy);
            await _personRepository.AddAsync(person, cancellationToken).ConfigureAwait(false);
            return person;
        }

        private async Task<Student> EnsureStudentAsync(int personId, string createdBy, CancellationToken cancellationToken)
        {
            var students = await _studentRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            var existingStudent = students.FirstOrDefault(s => s.PersonId == personId && !s.IsDeleted);
            if (existingStudent is not null)
            {
                return existingStudent;
            }

            var student = new Student(personId, 0, "GUEST", "Reserva web")
            {
                EnrollmentId = new EnrollmentId(GenerateEnrollmentId())
            };

            student.SetCreationInfo(createdBy);
            await _studentRepository.AddAsync(student, cancellationToken).ConfigureAwait(false);
            return student;
        }

        private async Task<Authorization> EnsureAuthorizationAsync(int studentId, string createdBy, CancellationToken cancellationToken)
        {
            var authorizations = await _authorizationRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            var validAuthorization = authorizations
                .Where(a => !a.IsDeleted && a.StudentId == studentId)
                .FirstOrDefault(a => a.IsValid());

            if (validAuthorization is not null)
            {
                return validAuthorization;
            }

            var authorization = new Authorization(
                studentId,
                DateTime.UtcNow.AddMinutes(-5),
                DateTime.UtcNow.AddDays(30),
                createdBy);

            await _authorizationRepository.AddAsync(authorization, cancellationToken).ConfigureAwait(false);
            return authorization;
        }

        private static string GenerateEnrollmentId()
        {
            var suffix = RandomNumberGenerator.GetInt32(1000, 9999);
            return $"{DateTime.UtcNow.Year}-{suffix}";
        }

        private async Task<string> GenerateUniqueGuestCedulaAsync(CancellationToken cancellationToken)
        {
            for (var attempt = 0; attempt < 5; attempt++)
            {
                var cedula = GenerateGuestCedula();
                var existingByCedula = await _personRepository.GetByCedulaAsync(cedula, cancellationToken).ConfigureAwait(false);
                if (existingByCedula is null)
                {
                    return cedula;
                }
            }

            // Last fallback is still 11 digits and very unlikely to collide.
            return GenerateGuestCedula();
        }

        private static string GenerateGuestCedula()
        {
            var numeric = RandomNumberGenerator.GetInt32(0, 10_000_000);
            var prefix = DateTime.UtcNow.ToString("yyMM");
            return $"{prefix}{numeric:0000000}";
        }

        private static string NormalizeName(string? value, string fallback)
        {
            var normalized = NormalizeOrNull(value);
            if (normalized is null)
            {
                return fallback;
            }

            return normalized.Length <= 50 ? normalized : normalized[..50];
        }

        private static string NormalizePhoneOrDefault(string? value)
        {
            var digits = value is null
                ? string.Empty
                : new string(value.Where(char.IsDigit).ToArray());

            if (digits.Length == 10)
            {
                return digits;
            }

            var suffix = RandomNumberGenerator.GetInt32(0, 10_000_000);
            return $"809{suffix:0000000}";
        }

        private static string GenerateGuestEmail()
        {
            var token = Guid.NewGuid().ToString("N")[..12];
            return $"guest-{token}@sga.local";
        }

        private static string? NormalizeOrNull(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}