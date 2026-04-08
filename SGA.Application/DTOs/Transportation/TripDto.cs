namespace SGA.Application.DTOs.Transportation
{
    public sealed record TripDto(
        int Id,
        int InstitutionId,
        int RouteId,
        string? RouteName,
        int DriverId,
        int BusId,
        string Status,
        DateTime ScheduledDepartureTime,
        DateTime ScheduledArrivalTime,
        DateTime? ActualDepartureTime,
        DateTime? ActualArrivalTime,
        int AvailableSeats);
}
