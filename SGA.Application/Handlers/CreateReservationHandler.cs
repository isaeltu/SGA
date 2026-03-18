using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Trips;
using SGA.Domain.Enums.Trips;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class CreateReservationHandler : IRequestHandler<CreateReservationCommand, int>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ITripRepository _tripRepository;
        private readonly IAuthorizationRepository _authorizationRepository;

        public CreateReservationHandler(
            IReservationRepository reservationRepository,
            ITripRepository tripRepository,
            IAuthorizationRepository authorizationRepository)
        {
            _reservationRepository = reservationRepository;
            _tripRepository = tripRepository;
            _authorizationRepository = authorizationRepository;
        }

        public async Task<int> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var trip = await _tripRepository.GetByIdAsync(request.TripId, cancellationToken).ConfigureAwait(false);
            if (trip is null)
            {
                throw new KeyNotFoundException($"Trip with id {request.TripId} was not found.");
            }

            if (trip.Status != TripStatus.Scheduled && trip.Status != TripStatus.InProgress)
            {
                throw new InvalidOperationException("Reservations are only allowed for scheduled or in-progress trips.");
            }

            var authorization = await _authorizationRepository.GetByIdAsync(request.AuthorizationId, cancellationToken).ConfigureAwait(false);
            if (authorization is null)
            {
                throw new KeyNotFoundException($"Authorization with id {request.AuthorizationId} was not found.");
            }

            if (!authorization.IsValid())
            {
                throw new InvalidOperationException("Authorization is not valid for reservation.");
            }

            var hasActiveReservation = await _reservationRepository
                .HasActiveReservationAsync(request.PersonId, request.TripId, cancellationToken)
                .ConfigureAwait(false);

            if (hasActiveReservation)
            {
                throw new InvalidOperationException("The user already has an active reservation for this trip.");
            }

            var tripReservations = await _reservationRepository
                .GetReservationsByTripIdAsync(request.TripId, cancellationToken)
                .ConfigureAwait(false);

            var occupiedSeats = tripReservations.Count(r => r.Status == ReservationStatus.Confirmed || r.Status == ReservationStatus.Boarded);
            if (occupiedSeats >= trip.AvailableSeats)
            {
                throw new InvalidOperationException("No seats available for this trip.");
            }

            var queueNumber = await _reservationRepository.GetNextQueueNumberAsync(request.TripId, cancellationToken).ConfigureAwait(false);

            var reservation = new Reservation(
                request.TripId,
                request.PersonId,
                request.AuthorizationId,
                queueNumber,
                request.CreatedBy);

            await _reservationRepository.AddAsync(reservation, cancellationToken).ConfigureAwait(false);
            return reservation.Id;
        }
    }
}
