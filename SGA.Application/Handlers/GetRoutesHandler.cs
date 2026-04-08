using MediatR;
using SGA.Application.DTOs.Transportation;
using SGA.Application.Queries;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class GetRoutesHandler : IRequestHandler<GetRoutesQuery, IReadOnlyCollection<RouteDto>>
    {
        private readonly IRouteRepository _routeRepository;

        public GetRoutesHandler(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<IReadOnlyCollection<RouteDto>> Handle(GetRoutesQuery request, CancellationToken cancellationToken)
        {
            var routes = await _routeRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);

            var query = routes.Where(r => !r.IsDeleted);
            if (request.InstitutionId.HasValue)
            {
                query = query.Where(r => r.InstitutionId == request.InstitutionId.Value);
            }

            return query
                .OrderBy(r => r.Name)
                .Select(r => new RouteDto(
                    r.Id,
                    r.InstitutionId,
                    r.Name,
                    r.Origin,
                    r.Destination,
                    r.DistanceKm,
                    r.EstimatedDurationMinutes,
                    r.IsActive))
                .ToArray();
        }
    }
}
