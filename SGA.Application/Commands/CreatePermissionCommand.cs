using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CreatePermissionCommand(
        string Name,
        string? Description,
        string CreatedBy) : IRequest<int>;
}