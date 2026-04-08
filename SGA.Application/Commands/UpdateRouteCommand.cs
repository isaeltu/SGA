using MediatR;

namespace SGA.Application.Commands
{
    public sealed record UpdateRouteCommand(
        int RouteId,
        int InstitutionId,
        string Name,
        string Origin,
        string Destination,
        decimal DistanceKm,
        int EstimatedDurationMinutes,
        bool IsActive,
        string ModifiedBy) : IRequest;
}
