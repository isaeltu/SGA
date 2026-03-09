using System.Text.Json;
using SGA.Application.Abstractions.Messaging;
using SGA.Domain.DomainEvents;

namespace SGA.Persistence.Messaging
{
    public class DomainEventSerializer : IDomainEventSerializer
    {
        private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
        {
            WriteIndented = false
        };

        public IntegrationMessage Serialize(IDomainEvent domainEvent)
        {
            return new IntegrationMessage(
                domainEvent.GetType().Name,
                JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), Options),
                domainEvent.OccurredOnUtc,
                "domain");
        }
    }
}
