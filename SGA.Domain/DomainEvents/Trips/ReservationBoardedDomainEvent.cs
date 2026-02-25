namespace SGA.Domain.DomainEvents.Trips
{
    public sealed record ReservationBoardedDomainEvent(int ReservationId, int TripId, int StudentId) : DomainEvent;
}
