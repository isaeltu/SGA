using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class CancelReservationHandler : IRequestHandler<CancelReservationCommand>
    {
        private readonly IReservationRepository _reservationRepository;

        public CancelReservationHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId, cancellationToken).ConfigureAwait(false);
            if (reservation is null)
            {
                throw new KeyNotFoundException($"Reservation with id {request.ReservationId} was not found.");
            }

            reservation.Cancel(request.ModifiedBy);
            await _reservationRepository.UpdateAsync(reservation, cancellationToken).ConfigureAwait(false);
        }
    }
}
