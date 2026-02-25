using System;

namespace SGA.Domain.DomainEvents
{
    public abstract record DomainEvent : IDomainEvent
    {
        protected DomainEvent()
        {
            OccurredOnUtc = DateTime.UtcNow;
        }

        public DateTime OccurredOnUtc { get; }
    }
}
