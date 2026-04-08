using MediatR;
using SGA.Application.DTOs.Transportation;

namespace SGA.Application.Queries
{
    public sealed record GetRoutesQuery(int? InstitutionId = null) : IRequest<IReadOnlyCollection<RouteDto>>;
}
