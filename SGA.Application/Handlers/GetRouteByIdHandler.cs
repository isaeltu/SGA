using MediatR;
using SGA.Application.DTOs.Transportation;
using SGA.Application.Queries;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class GetRouteByIdHandler : IRequestHandler<GetRouteByIdQuery, RouteDto?>
    {
        private readonly IRouteRepository _routeRepository;

        public GetRouteByIdHandler(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<RouteDto?> Handle(GetRouteByIdQuery request, CancellationToken cancellationToken)
        {
            var route = await _routeRepository.GetByIdAsync(request.RouteId, cancellationToken).ConfigureAwait(false);
            if (route is null || route.IsDeleted)
            {
                return null;
            }

            return new RouteDto(
                route.Id,
                route.InstitutionId,
                route.Name,
                route.Origin,
                route.Destination,
                route.DistanceKm,
                route.EstimatedDurationMinutes,
                route.IsActive);
        }
    }
}
