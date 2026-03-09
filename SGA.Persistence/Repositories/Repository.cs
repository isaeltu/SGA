using Microsoft.EntityFrameworkCore;
using SGA.Domain.Base;
using SGA.Domain.Repositories.Base;
using SGA.Persistence.Context;

namespace SGA.Persistence.Repositories
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : BaseEntity<TId>
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<TEntity> Entities;

        public Repository(ApplicationDbContext context)
        {
            Context = context;
            Entities = context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entity;
        }

        public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken)
        {
            return await Entities.FindAsync([id], cancellationToken).ConfigureAwait(false);
        }

        public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await Entities.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            Entities.Update(entity);
            return await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
        }

        public async Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken)
        {
            var entity = await GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (entity is null)
            {
                return false;
            }

            entity.IsDeleted = true;
            return await UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
        }

        public async Task SaveChangeAsync(CancellationToken cancellationToken)
        {
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
