using SGA.Domain.Base;
using SGA.Domain.Common;
using SGA.Domain.Services.Interfaces.MultiTenancy;

namespace SGA.Domain.Services.MultiTenancy
{
    public class MultiTenantDomainService : IMultiTenantDomainService
    {
        public Result EnsureTenantAccess<T>(T entity, int tenantId) where T : IMultiTenantEntity
        {
            if (entity.InstitutionId != tenantId)
            {
                return Result.Failure(new Error("MultiTenant.Forbidden", "La entidad no pertenece al tenant actual."));
            }

            return Result.Success();
        }

        public IQueryable<T> ApplyTenantFilter<T>(IQueryable<T> query, int tenantId) where T : class, IMultiTenantEntity
        {
            return query.Where(x => x.InstitutionId == tenantId);
        }
    }
}