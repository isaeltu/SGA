using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CreatePersonCommand(
        int InstitutionId,
        int RoleId,
        string Email,
        string PhoneNumber,
        string FirstName,
        string LastName,
        string Cedula,
        string CreatedBy) : IRequest<int>;
}
