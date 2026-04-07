using System.Security.Cryptography;
using MediatR;
using SGA.Application.Commands;
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
            var existingByEmail = await _personRepository.GetByEmailAsync(request.Email, cancellationToken).ConfigureAwait(false);
            if (existingByEmail is not null)
            {
                return existingByEmail;
            }

            var person = new Person(
                roleId,
                request.Email,
                request.PhoneNumber,
                request.FirstName,
                request.LastName,
                GenerateGuestCedula(request.PhoneNumber))
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

        private static string GenerateGuestCedula(string phoneNumber)
        {
            var digits = new string(phoneNumber.Where(char.IsDigit).ToArray());
            var tail = digits.Length >= 6 ? digits[^6..] : digits.PadLeft(6, '0');
            return $"GUEST-{tail}-{DateTime.UtcNow:HHmmss}";
        }
    }
}