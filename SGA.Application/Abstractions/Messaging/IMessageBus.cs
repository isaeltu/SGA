namespace SGA.Application.Abstractions.Messaging
{
    public interface IMessageBus
    {
        Task PublishAsync(IntegrationMessage message, CancellationToken cancellationToken = default);
    }
}
