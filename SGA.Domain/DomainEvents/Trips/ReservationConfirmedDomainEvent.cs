namespace SGA.Domain.DomainEvents.Trips
{
    public sealed record ReservationConfirmedDomainEvent(int ReservationId, int TripId, int StudentId) : DomainEvent;
}
