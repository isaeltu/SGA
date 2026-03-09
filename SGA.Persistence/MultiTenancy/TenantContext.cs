using SGA.Application.Abstractions.MultiTenancy;

namespace SGA.Persistence.MultiTenancy
{
    public class TenantContext : ITenantContext
    {
        private static readonly AsyncLocal<int?> CurrentTenant = new();

        public int TenantId => CurrentTenant.Value ?? 0;

        public void SetTenant(int tenantId)
        {
            CurrentTenant.Value = tenantId;
        }
    }
}
