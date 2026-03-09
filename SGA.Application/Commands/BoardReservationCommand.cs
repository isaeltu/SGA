using MediatR;

namespace SGA.Application.Commands
{
    public sealed record BoardReservationCommand(int ReservationId, string ModifiedBy) : IRequest;
}
