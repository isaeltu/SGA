using MediatR;

namespace SGA.Application.Commands
{
    public sealed record UpdateRoleCommand(
        byte RoleId,
        string Name,
        string? Description,
        string ModifiedBy) : IRequest;
}
