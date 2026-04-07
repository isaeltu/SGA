using MediatR;
using SGA.Application.DTOs.Transportation;

namespace SGA.Application.Queries
{
    public sealed record GetBusByIdQuery(int BusId) : IRequest<BusDto?>;
}
