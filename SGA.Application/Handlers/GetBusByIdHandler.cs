using MediatR;
using SGA.Application.DTOs.Transportation;
using SGA.Application.Queries;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class GetBusByIdHandler : IRequestHandler<GetBusByIdQuery, BusDto?>
    {
        private readonly IBusRepository _busRepository;

        public GetBusByIdHandler(IBusRepository busRepository)
        {
            _busRepository = busRepository;
        }

        public async Task<BusDto?> Handle(GetBusByIdQuery request, CancellationToken cancellationToken)
        {
            var bus = await _busRepository.GetByIdAsync(request.BusId, cancellationToken).ConfigureAwait(false);
            if (bus is null)
            {
                return null;
            }

            return new BusDto(
                bus.Id,
                bus.InstitutionId,
                bus.LicensePlate.Value,
                bus.Model,
                bus.Year,
                bus.Capacity,
                bus.AvailableSeats,
                bus.Status.ToString());
        }
    }
}
