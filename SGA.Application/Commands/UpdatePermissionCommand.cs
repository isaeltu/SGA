using MediatR;

namespace SGA.Application.Commands
{
    public sealed record UpdatePermissionCommand(
        int PermissionId,
        string Name,
        string? Description,
        string ModifiedBy) : IRequest;
}
