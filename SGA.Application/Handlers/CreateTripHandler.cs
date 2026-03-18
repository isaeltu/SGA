using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Trips;
using SGA.Domain.Repositories;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class CreateTripHandler : IRequestHandler<CreateTripCommand, int>
    {
        private readonly ITripRepository _tripRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IBusRepository _busRepository;

        public CreateTripHandler(
            ITripRepository tripRepository,
            IRouteRepository routeRepository,
            IDriverRepository driverRepository,
            IBusRepository busRepository)
        {
            _tripRepository = tripRepository;
            _routeRepository = routeRepository;
            _driverRepository = driverRepository;
            _busRepository = busRepository;
        }

        public async Task<int> Handle(CreateTripCommand request, CancellationToken cancellationToken)
        {
            var route = await _routeRepository.GetByIdAsync(request.RouteId, cancellationToken).ConfigureAwait(false);
            if (route is null)
            {
                throw new KeyNotFoundException($"Route with id {request.RouteId} was not found.");
            }

            var driver = await _driverRepository.GetByIdAsync(request.DriverId, cancellationToken).ConfigureAwait(false);
            if (driver is null)
            {
                throw new KeyNotFoundException($"Driver with id {request.DriverId} was not found.");
            }

            var bus = await _busRepository.GetByIdAsync(request.BusId, cancellationToken).ConfigureAwait(false);
            if (bus is null)
            {
                throw new KeyNotFoundException($"Bus with id {request.BusId} was not found.");
            }

            var availableSeats = request.AvailableSeats ?? bus.AvailableSeats;
            if (availableSeats <= 0)
            {
                throw new InvalidOperationException("Available seats must be greater than zero.");
            }

            var trip = new Trip(
                request.RouteId,
                request.DriverId,
                request.BusId,
                request.ScheduledDepartureTime,
                request.ScheduledArrivalTime,
                availableSeats,
                request.CreatedBy)
            {
                InstitutionId = request.InstitutionId ?? bus.InstitutionId
            };

            await _tripRepository.AddAsync(trip, cancellationToken).ConfigureAwait(false);
            return trip.Id;
        }
    }
}
