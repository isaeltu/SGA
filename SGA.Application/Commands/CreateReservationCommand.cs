using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CreateReservationCommand(
        int TripId,
        int PersonId,
        int AuthorizationId,
        string CreatedBy) : IRequest<int>;
}
