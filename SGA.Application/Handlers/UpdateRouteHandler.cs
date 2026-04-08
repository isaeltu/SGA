using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Repositories;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class UpdateRouteHandler : IRequestHandler<UpdateRouteCommand>
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IInstitutionRepository _institutionRepository;

        public UpdateRouteHandler(IRouteRepository routeRepository, IInstitutionRepository institutionRepository)
        {
            _routeRepository = routeRepository;
            _institutionRepository = institutionRepository;
        }

        public async Task Handle(UpdateRouteCommand request, CancellationToken cancellationToken)
        {
            var route = await _routeRepository.GetByIdAsync(request.RouteId, cancellationToken).ConfigureAwait(false);
            if (route is null || route.IsDeleted)
            {
                throw new KeyNotFoundException($"Route with id {request.RouteId} was not found.");
            }

            var institution = await _institutionRepository.GetByIdAsync(request.InstitutionId, cancellationToken).ConfigureAwait(false);
            if (institution is null)
            {
                throw new KeyNotFoundException($"Institution with id {request.InstitutionId} was not found.");
            }

            route.InstitutionId = request.InstitutionId;
            route.UpdateDetails(
                request.Name,
                request.Origin,
                request.Destination,
                request.DistanceKm,
                request.EstimatedDurationMinutes,
                request.ModifiedBy);

            if (request.IsActive)
            {
                route.Activate(request.ModifiedBy);
            }
            else if (route.IsActive)
            {
                route.Deactivate(request.ModifiedBy);
            }

            await _routeRepository.UpdateAsync(route, cancellationToken).ConfigureAwait(false);
        }
    }
}
