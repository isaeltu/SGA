using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class CancelTripHandler : IRequestHandler<CancelTripCommand>
    {
        private readonly ITripRepository _tripRepository;

        public CancelTripHandler(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task Handle(CancelTripCommand request, CancellationToken cancellationToken)
        {
            var trip = await _tripRepository.GetByIdAsync(request.TripId, cancellationToken).ConfigureAwait(false);
            if (trip is null)
            {
                throw new KeyNotFoundException($"Trip with id {request.TripId} was not found.");
            }

            var result = trip.Cancel(request.ModifiedBy);
            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error.Message);
            }

            await _tripRepository.UpdateAsync(trip, cancellationToken).ConfigureAwait(false);
        }
    }
}
