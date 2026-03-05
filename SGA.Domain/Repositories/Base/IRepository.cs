using System.Numerics;
using SGA.Domain.Base;

namespace SGA.Domain.Repositories.Base
{
    public interface IRepository<TEntity, TId>
    where TEntity : class, IAuditEntity
    {
      
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity?> DeleteAsync(TId id, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
