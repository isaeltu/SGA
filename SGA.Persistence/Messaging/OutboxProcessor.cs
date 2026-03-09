using Microsoft.EntityFrameworkCore;
using SGA.Application.Abstractions.Messaging;
using SGA.Persistence.Context;

namespace SGA.Persistence.Messaging
{
    public class OutboxProcessor : IOutboxProcessor
    {
        private readonly ApplicationDbContext _context;
        private readonly IMessageBus _messageBus;

        public OutboxProcessor(ApplicationDbContext context, IMessageBus messageBus)
        {
            _context = context;
            _messageBus = messageBus;
        }

        public async Task<int> PublishPendingAsync(CancellationToken cancellationToken = default)
        {
            var pending = await _context.OutboxMessages
                .Where(x => x.ProcessedOnUtc == null)
                .OrderBy(x => x.OccurredOnUtc)
                .Take(500)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach (var message in pending)
            {
                try
                {
                    await _messageBus.PublishAsync(
                        new IntegrationMessage(message.MessageType, message.Payload, message.OccurredOnUtc, "outbox"),
                        cancellationToken).ConfigureAwait(false);

                    message.ProcessedOnUtc = DateTime.UtcNow;
                    message.Error = null;
                }
                catch (Exception ex)
                {
                    message.Error = ex.Message;
                }
            }

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return pending.Count;
        }
    }
}
