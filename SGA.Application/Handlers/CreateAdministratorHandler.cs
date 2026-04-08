using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class CreateAdministratorHandler : IRequestHandler<CreateAdministratorCommand, int>
    {
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IPersonRepository _personRepository;

        public CreateAdministratorHandler(IAdministratorRepository administratorRepository, IPersonRepository personRepository)
        {
            _administratorRepository = administratorRepository;
            _personRepository = personRepository;
        }

        public async Task<int> Handle(CreateAdministratorCommand request, CancellationToken cancellationToken)
        {
            var personExists = await _personRepository.ExistsByIdAsync(request.PersonId, cancellationToken).ConfigureAwait(false);
            if (!personExists)
            {
                throw new KeyNotFoundException($"Person with id {request.PersonId} was not found.");
            }

            var administrator = new Administrator(request.PersonId, request.AdminLevel, request.CreatedBy);
            administrator.SetCreationInfo(request.CreatedBy);
            await _administratorRepository.AddAsync(administrator, cancellationToken).ConfigureAwait(false);
            return administrator.Id;
        }
    }
}
