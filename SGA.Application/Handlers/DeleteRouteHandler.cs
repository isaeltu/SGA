using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class DeleteRouteHandler : IRequestHandler<DeleteRouteCommand>
    {
        private readonly IRouteRepository _routeRepository;

        public DeleteRouteHandler(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _routeRepository.DeleteAsync(request.RouteId, cancellationToken).ConfigureAwait(false);
            if (!deleted)
            {
                throw new KeyNotFoundException($"Route with id {request.RouteId} was not found.");
            }
        }
    }
}
