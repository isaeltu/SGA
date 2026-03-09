using MediatR;

namespace SGA.Application.Commands
{
    public sealed record StartTripCommand(int TripId, string ModifiedBy) : IRequest;
}
