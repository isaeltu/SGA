using MediatR;
using SGA.Application.DTOs.Transportation;
using SGA.Application.Queries;
using SGA.Domain.Enums.Trips;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class GetTripsHandler : IRequestHandler<GetTripsQuery, IReadOnlyCollection<TripDto>>
    {
        private readonly ITripRepository _tripRepository;

        public GetTripsHandler(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<IReadOnlyCollection<TripDto>> Handle(GetTripsQuery request, CancellationToken cancellationToken)
        {
            var trips = await _tripRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);

            var query = trips.Where(t => !t.IsDeleted);

            if (request.InstitutionId.HasValue)
            {
                query = query.Where(t => t.InstitutionId == request.InstitutionId.Value);
            }

            if (request.OnlyBookable)
            {
                query = query.Where(t =>
                    (t.Status == TripStatus.Scheduled || t.Status == TripStatus.InProgress)
                    && t.AvailableSeats > 0);
            }

            return query
                .OrderBy(t => t.ScheduledDepartureTime)
                .Select(t => new TripDto(
                    t.Id,
                    t.InstitutionId,
                    t.RouteId,
                    t.DriverId,
                    t.BusId,
                    t.Status.ToString(),
                    t.ScheduledDepartureTime,
                    t.ScheduledArrivalTime,
                    t.ActualDepartureTime,
                    t.ActualArrivalTime,
                    t.AvailableSeats))
                .ToArray();
        }
    }
}