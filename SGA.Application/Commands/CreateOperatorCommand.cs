using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CreateOperatorCommand(
        int PersonId,
        string AssignedArea,
        int ShiftNumber,
        string CreatedBy) : IRequest<int>;
}
