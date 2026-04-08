namespace SGA.Application.DTOs.Transportation
{
    public sealed record RouteDto(
        int Id,
        int InstitutionId,
        string Name,
        string Origin,
        string Destination,
        decimal DistanceKm,
        int EstimatedDurationMinutes,
        bool IsActive);
}
