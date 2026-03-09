using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class BoardReservationHandler : IRequestHandler<BoardReservationCommand>
    {
        private readonly IReservationRepository _reservationRepository;

        public BoardReservationHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task Handle(BoardReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId, cancellationToken).ConfigureAwait(false);
            if (reservation is null)
            {
                throw new KeyNotFoundException($"Reservation with id {request.ReservationId} was not found.");
            }

            reservation.MarkAsBoarded(request.ModifiedBy);
            await _reservationRepository.UpdateAsync(reservation, cancellationToken).ConfigureAwait(false);
        }
    }
}
