using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CreateModeCommand(
        string Name,
        string CreatedBy) : IRequest<int>;
}