using System.Numerics;
using SGA.Domain.Base;

namespace SGA.Domain.Repositories.Base
{
    public interface IRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
    {
      
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken);
        Task SaveChangeAsync(CancellationToken cancellationToken);
    }
}
