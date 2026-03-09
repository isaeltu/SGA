using SGA.Domain.DomainEvents;

namespace SGA.Application.Abstractions.Messaging
{
    public interface IDomainEventSerializer
    {
        IntegrationMessage Serialize(IDomainEvent domainEvent);
    }
}
