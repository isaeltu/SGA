using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Transportation;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class UpdateBusHandler : IRequestHandler<UpdateBusCommand>
    {
        private readonly IBusRepository _busRepository;

        public UpdateBusHandler(IBusRepository busRepository)
        {
            _busRepository = busRepository;
        }

        public async Task Handle(UpdateBusCommand request, CancellationToken cancellationToken)
        {
            var bus = await _busRepository.GetByIdAsync(request.BusId, cancellationToken).ConfigureAwait(false);
            if (bus is null || bus.IsDeleted)
            {
                throw new KeyNotFoundException($"Bus with id {request.BusId} was not found.");
            }

            bus.InstitutionId = request.InstitutionId;
            bus.LicensePlate = new LicensePlate(request.LicensePlate);
            bus.Model = request.Model.Trim();
            bus.Year = request.Year;
            bus.Capacity = request.Capacity;
            bus.AvailableSeats = request.Capacity;
            bus.SetModificationInfo(request.ModifiedBy);

            await _busRepository.UpdateAsync(bus, cancellationToken).ConfigureAwait(false);
        }
    }
}
