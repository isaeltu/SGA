using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SGA.Domain.Base;
using SGA.Domain.Repositories.Base;
using SGA.Persistence.Context;

namespace SGA.Persistence.Repositories
{
    namespace SGA.Persistence.Repositories
    {
        public class Repository<TEntity, TId> : IRepository<TEntity, TId>
             where TEntity : BaseEntity<TId>, IAuditEntity
        {
            private readonly ApplicationDbContext _context;
            private readonly DbSet<TEntity> _entities;

            public Repository(ApplicationDbContext context)
            {
                _context = context;
                _entities = context.Set<TEntity>();
            }

            public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
            {
                await _entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return entity;
            }

           
            public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken)
            {
                
                return await _entities
                    .FindAsync(new object?[] { id }, cancellationToken)
                    .ConfigureAwait(false);
            }

            public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken)
            {
                return await _entities
                    .AsNoTracking()                         
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
            }


            public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
            {
                bool exists = await _entities
                    .AnyAsync(e => e.Id!.Equals(entity.Id), cancellationToken)
                    .ConfigureAwait(false);

                if (!exists)
                    throw new KeyNotFoundException(
                        $"{typeof(TEntity).Name} with id '{entity.Id}' was not found.");

                _context.Entry(entity).State = EntityState.Modified; 
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return entity;
            }

            public async Task<TEntity?> DeleteAsync(TId id, CancellationToken cancellationToken)
            {
                var entity = await _entities
                    .FindAsync(new object?[] { id }, cancellationToken)
                    .ConfigureAwait(false);

                if (entity is null) return null;

                _entities.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return entity;
            }
            public async Task SaveAsync(CancellationToken cancellationToken)
            {
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}