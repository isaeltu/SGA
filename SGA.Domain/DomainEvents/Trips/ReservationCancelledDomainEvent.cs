namespace SGA.Domain.DomainEvents.Trips
{
    public sealed record ReservationCancelledDomainEvent(int ReservationId, int TripId, int StudentId) : DomainEvent;
}
