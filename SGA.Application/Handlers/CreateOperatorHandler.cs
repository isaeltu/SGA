using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class CreateOperatorHandler : IRequestHandler<CreateOperatorCommand, int>
    {
        private readonly IOperatorRepository _operatorRepository;
        private readonly IPersonRepository _personRepository;

        public CreateOperatorHandler(IOperatorRepository operatorRepository, IPersonRepository personRepository)
        {
            _operatorRepository = operatorRepository;
            _personRepository = personRepository;
        }

        public async Task<int> Handle(CreateOperatorCommand request, CancellationToken cancellationToken)
        {
            var personExists = await _personRepository.ExistsByIdAsync(request.PersonId, cancellationToken).ConfigureAwait(false);
            if (!personExists)
            {
                throw new KeyNotFoundException($"Person with id {request.PersonId} was not found.");
            }

            var op = new Operator(request.PersonId, request.AssignedArea, request.ShiftNumber);
            op.SetCreationInfo(request.CreatedBy);
            await _operatorRepository.AddAsync(op, cancellationToken).ConfigureAwait(false);
            return op.Id;
        }
    }
}
