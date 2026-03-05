
namespace SGA.Domain.DomainEvents.Trips
{
    public sealed record ReservationBoardedDomainEvent(
        int ReservationId,
        int TripId,
        int StudentId) : IDomainEvent
    {
        
        public DateTime OccurredOnUtc => DateTime.UtcNow;
    }
}
