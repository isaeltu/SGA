namespace SGA.Domain.DomainEvents.Trips
{
    public sealed record TripStartedDomainEvent(int TripId) : DomainEvent;
}
