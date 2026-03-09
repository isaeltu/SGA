namespace SGA.Application.Abstractions.Messaging
{
    public interface IOutboxProcessor
    {
        Task<int> PublishPendingAsync(CancellationToken cancellationToken = default);
    }
}
