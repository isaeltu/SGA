using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class CompleteTripHandler : IRequestHandler<CompleteTripCommand>
    {
        private readonly ITripRepository _tripRepository;

        public CompleteTripHandler(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task Handle(CompleteTripCommand request, CancellationToken cancellationToken)
        {
            var trip = await _tripRepository.GetByIdAsync(request.TripId, cancellationToken).ConfigureAwait(false);
            if (trip is null)
            {
                throw new KeyNotFoundException($"Trip with id {request.TripId} was not found.");
            }

            var result = trip.Complete(request.ModifiedBy);
            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error.Message);
            }

            await _tripRepository.UpdateAsync(trip, cancellationToken).ConfigureAwait(false);
        }
    }
}
