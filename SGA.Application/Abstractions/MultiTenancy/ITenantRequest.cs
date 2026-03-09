namespace SGA.Application.Abstractions.MultiTenancy
{
    public interface ITenantRequest
    {
        int TenantId { get; }
    }
}