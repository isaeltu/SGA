using System;

namespace SGA.Persistence.Entities
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public string MessageType { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public DateTime OccurredOnUtc { get; set; }
        public DateTime? ProcessedOnUtc { get; set; }
        public string? Error { get; set; }
    }
}
