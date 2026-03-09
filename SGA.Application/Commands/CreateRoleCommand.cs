using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CreateRoleCommand(
        string Name,
        string? Description,
        string CreatedBy) : IRequest<byte>;
}