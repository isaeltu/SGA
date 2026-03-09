using MediatR;
using SGA.Application.DTOs.Trips;
using SGA.Application.Queries;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class GetReservationByIdHandler : IRequestHandler<GetReservationByIdQuery, ReservationDto?>
    {
        private readonly IReservationRepository _reservationRepository;

        public GetReservationByIdHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<ReservationDto?> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId, cancellationToken).ConfigureAwait(false);
            if (reservation is null)
            {
                return null;
            }

            return new ReservationDto(
                reservation.Id,
                reservation.TripId,
                reservation.PersonId,
                reservation.AuthorizationId,
                reservation.QueueNumber,
                reservation.QrCode,
                reservation.Status.ToString(),
                reservation.CreatedAt);
        }
    }
}
