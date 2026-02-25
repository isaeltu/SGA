using System;

namespace SGA.Domain.DomainEvents
{
    public interface IDomainEvent
    {
        DateTime OccurredOnUtc { get; }
    }
}
