using MediatR;
using SGA.Application.DTOs.Transportation;

namespace SGA.Application.Queries
{
    public sealed record GetRouteByIdQuery(int RouteId) : IRequest<RouteDto?>;
}
