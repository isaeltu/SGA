using MediatR;

namespace SGA.Application.Commands
{
    public sealed record UpdateModeCommand(
        int ModeId,
        string Name,
        string ModifiedBy) : IRequest;
}
