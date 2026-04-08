using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Transportation;
using SGA.Domain.Repositories;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class CreateRouteHandler : IRequestHandler<CreateRouteCommand, int>
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IInstitutionRepository _institutionRepository;

        public CreateRouteHandler(IRouteRepository routeRepository, IInstitutionRepository institutionRepository)
        {
            _routeRepository = routeRepository;
            _institutionRepository = institutionRepository;
        }

        public async Task<int> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
        {
            var institution = await _institutionRepository.GetByIdAsync(request.InstitutionId, cancellationToken).ConfigureAwait(false);
            if (institution is null)
            {
                throw new KeyNotFoundException($"Institution with id {request.InstitutionId} was not found.");
            }

            var route = new Route(
                request.Name,
                request.Origin,
                request.Destination,
                request.DistanceKm,
                request.EstimatedDurationMinutes,
                request.CreatedBy)
            {
                InstitutionId = request.InstitutionId
            };

            await _routeRepository.AddAsync(route, cancellationToken).ConfigureAwait(false);
            return route.Id;
        }
    }
}
