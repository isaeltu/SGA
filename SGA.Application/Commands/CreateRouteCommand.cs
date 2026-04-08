using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CreateRouteCommand(
        int InstitutionId,
        string Name,
        string Origin,
        string Destination,
        decimal DistanceKm,
        int EstimatedDurationMinutes,
        string CreatedBy) : IRequest<int>;
}
