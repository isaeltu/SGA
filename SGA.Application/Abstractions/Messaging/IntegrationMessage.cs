namespace SGA.Application.Abstractions.Messaging
{
    public sealed record IntegrationMessage(
        string MessageType,
        string Payload,
        DateTime OccurredOnUtc,
        string Source);
}
