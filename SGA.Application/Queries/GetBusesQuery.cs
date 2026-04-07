using MediatR;
using SGA.Application.DTOs.Transportation;

namespace SGA.Application.Queries
{
    public sealed record GetBusesQuery : IRequest<IReadOnlyCollection<BusDto>>;
}
