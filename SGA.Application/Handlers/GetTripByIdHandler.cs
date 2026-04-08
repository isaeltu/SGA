using MediatR;
using SGA.Application.DTOs.Transportation;
using SGA.Application.Queries;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class GetTripByIdHandler : IRequestHandler<GetTripByIdQuery, TripDto?>
    {
        private readonly ITripRepository _tripRepository;
        private readonly IRouteRepository _routeRepository;

        public GetTripByIdHandler(ITripRepository tripRepository, IRouteRepository routeRepository)
        {
            _tripRepository = tripRepository;
            _routeRepository = routeRepository;
        }

        public async Task<TripDto?> Handle(GetTripByIdQuery request, CancellationToken cancellationToken)
        {
            var trip = await _tripRepository.GetByIdAsync(request.TripId, cancellationToken).ConfigureAwait(false);
            if (trip is null)
            {
                return null;
            }

            var route = await _routeRepository.GetByIdAsync(trip.RouteId, cancellationToken).ConfigureAwait(false);

            return new TripDto(
                trip.Id,
                trip.InstitutionId,
                trip.RouteId,
                route?.Name,
                trip.DriverId,
                trip.BusId,
                trip.Status.ToString(),
                trip.ScheduledDepartureTime,
                trip.ScheduledArrivalTime,
                trip.ActualDepartureTime,
                trip.ActualArrivalTime,
                trip.AvailableSeats);
        }
    }
}
