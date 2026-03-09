using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CancelTripCommand(int TripId, string ModifiedBy) : IRequest;
}
