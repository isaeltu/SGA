using SGA.Domain.Base;
using SGA.Domain.Common;

namespace SGA.Domain.Services.Interfaces.MultiTenancy
{
    public interface IMultiTenantDomainService
    {
        Result EnsureTenantAccess<T>(T entity, int tenantId) where T : IMultiTenantEntity;
        IQueryable<T> ApplyTenantFilter<T>(IQueryable<T> query, int tenantId) where T : class, IMultiTenantEntity;
    }
}