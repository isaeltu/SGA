using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Transportation;
using SGA.Domain.Repositories;

namespace SGA.Application.Handlers
{
    public sealed class CreateBusHandler : IRequestHandler<CreateBusCommand, int>
    {
        private readonly IBusRepository _busRepository;

        public CreateBusHandler(IBusRepository busRepository)
        {
            _busRepository = busRepository;
        }

        public async Task<int> Handle(CreateBusCommand request, CancellationToken cancellationToken)
        {
            var bus = new Bus(
                request.LicensePlate,
                request.Model,
                request.Year,
                request.Capacity,
                request.CreatedBy)
            {
                InstitutionId = request.InstitutionId
            };

            await _busRepository.AddAsync(bus, cancellationToken).ConfigureAwait(false);
            return bus.Id;
        }
    }
}
