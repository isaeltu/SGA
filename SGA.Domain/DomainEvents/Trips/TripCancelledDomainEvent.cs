namespace SGA.Domain.DomainEvents.Trips
{
    public sealed record TripCancelledDomainEvent(int TripId) : DomainEvent;
}
