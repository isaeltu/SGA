namespace SGA.Domain.DomainEvents.Trips
{
    public sealed record TripCompletedDomainEvent(int TripId) : DomainEvent;
}
