using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CancelReservationCommand(int ReservationId, string ModifiedBy) : IRequest;
}
