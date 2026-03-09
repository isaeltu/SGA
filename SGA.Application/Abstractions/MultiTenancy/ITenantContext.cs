namespace SGA.Application.Abstractions.MultiTenancy
{
    public interface ITenantContext
    {
        int TenantId { get; }
        void SetTenant(int tenantId);
    }
}