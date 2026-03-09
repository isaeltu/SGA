using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CompleteTripCommand(int TripId, string ModifiedBy) : IRequest;
}
