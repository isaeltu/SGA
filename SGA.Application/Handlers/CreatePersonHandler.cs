using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class CreatePersonHandler : IRequestHandler<CreatePersonCommand, int>
    {
        private readonly IPersonRepository _personRepository;

        public CreatePersonHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<int> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var existingByEmail = await _personRepository.GetByEmailAsync(request.Email, cancellationToken).ConfigureAwait(false);
            if (existingByEmail is not null)
            {
                return existingByEmail.Id;
            }

            var existingByCedula = await _personRepository.GetByCedulaAsync(request.Cedula, cancellationToken).ConfigureAwait(false);
            if (existingByCedula is not null)
            {
                return existingByCedula.Id;
            }

            var person = new Person(
                request.RoleId,
                request.Email,
                request.PhoneNumber,
                request.FirstName,
                request.LastName,
                request.Cedula)
            {
                InstitutionId = request.InstitutionId
            };

            person.SetCreationInfo(request.CreatedBy);
            await _personRepository.AddAsync(person, cancellationToken).ConfigureAwait(false);
            return person.Id;
        }
    }
}
