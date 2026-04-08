using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPersonRepository _personRepository;

        public CreateEmployeeHandler(IEmployeeRepository employeeRepository, IPersonRepository personRepository)
        {
            _employeeRepository = employeeRepository;
            _personRepository = personRepository;
        }

        public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var personExists = await _personRepository.ExistsByIdAsync(request.PersonId, cancellationToken).ConfigureAwait(false);
            if (!personExists)
            {
                throw new KeyNotFoundException($"Person with id {request.PersonId} was not found.");
            }

            var employee = new Employee(
                request.PersonId,
                request.DepartmentId,
                request.EmployeeCode,
                request.Position,
                request.HireDate);

            employee.SetCreationInfo(request.CreatedBy);
            await _employeeRepository.AddAsync(employee, cancellationToken).ConfigureAwait(false);
            return employee.Id;
        }
    }
}
