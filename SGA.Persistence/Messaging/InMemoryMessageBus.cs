using Microsoft.Extensions.Logging;
using SGA.Application.Abstractions.Messaging;

namespace SGA.Persistence.Messaging
{
    public class InMemoryMessageBus : IMessageBus
    {
        private readonly ILogger<InMemoryMessageBus> _logger;

        public InMemoryMessageBus(ILogger<InMemoryMessageBus> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync(IntegrationMessage message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Published integration message {MessageType} at {OccurredOnUtc} from {Source}",
                message.MessageType,
                message.OccurredOnUtc,
                message.Source);

            return Task.CompletedTask;
        }
    }
}
