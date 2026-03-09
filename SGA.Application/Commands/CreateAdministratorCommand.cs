using MediatR;
using SGA.Domain.Enums.Users;

namespace SGA.Application.Commands
{
    public sealed record CreateAdministratorCommand(
        int PersonId,
        AdminLevel AdminLevel,
        string CreatedBy) : IRequest<int>;
}
