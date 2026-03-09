namespace SGA.Application.DTOs.Trips
{
    public sealed record ReservationDto(
        int Id,
        int TripId,
        int PersonId,
        int AuthorizationId,
        int QueueNumber,
        string QrCode,
        string Status,
        DateTime CreatedAt);
}
