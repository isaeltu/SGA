using MediatR;
using SGA.Application.DTOs.Transportation;
using SGA.Application.Queries;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class GetBusesHandler : IRequestHandler<GetBusesQuery, IReadOnlyCollection<BusDto>>
    {
        private readonly IBusRepository _busRepository;

        public GetBusesHandler(IBusRepository busRepository)
        {
            _busRepository = busRepository;
        }

        public async Task<IReadOnlyCollection<BusDto>> Handle(GetBusesQuery request, CancellationToken cancellationToken)
        {
            var buses = await _busRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            return buses
                .Where(bus => !bus.IsDeleted)
                .Select(bus => new BusDto(
                    bus.Id,
                    bus.InstitutionId,
                    bus.LicensePlate.Value,
                    bus.Model,
                    bus.Year,
                    bus.Capacity,
                    bus.AvailableSeats,
                    bus.Status.ToString()))
                .ToArray();
        }
    }
}
