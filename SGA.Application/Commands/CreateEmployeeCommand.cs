using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CreateEmployeeCommand(
        int PersonId,
        int DepartmentId,
        string EmployeeCode,
        string Position,
        DateTimeOffset HireDate,
        string CreatedBy) : IRequest<int>;
}
