using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class DeleteBusHandler : IRequestHandler<DeleteBusCommand>
    {
        private readonly IBusRepository _busRepository;

        public DeleteBusHandler(IBusRepository busRepository)
        {
            _busRepository = busRepository;
        }

        public async Task Handle(DeleteBusCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _busRepository.DeleteAsync(request.BusId, cancellationToken).ConfigureAwait(false);
            if (!deleted)
            {
                throw new KeyNotFoundException($"Bus with id {request.BusId} was not found.");
            }
        }
    }
}