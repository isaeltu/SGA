using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SGA.Domain.Base;
using SGA.Domain.Common;
using SGA.Domain.Repositories.Base;
using SGA.Persistence.Context;

namespace SGA.Persistence.Repositories
{
    namespace SGA.Persistence.Repositories
    {
        public class Repository<TEntity, TId> : IRepository<TEntity, TId>
             where TEntity : BaseEntity<TId>, IAuditEntity
        {
            public readonly ApplicationDbContext _context;
            public readonly DbSet<TEntity> _entities;

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
                
                return await _entities.
                    FindAsync(id, cancellationToken).
                    ConfigureAwait(false);
            }

            public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken)
            {
                return await _entities
                    .AsNoTracking()                         
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
            }


            public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
            {
                try
                {
                    _entities.Update(entity);
                   return await _context.SaveChangesAsync(cancellationToken) > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return false;
            }

            public async Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken)
            {
                try
                {
                    TEntity? entity = await GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
                    entity!.IsDeleted = true;
                    return await UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex )
                {
                    Result<TEntity> result;
                    
                }
                return false;
            }
            public async Task SaveChangeAsync(CancellationToken cancellationToken)
            {
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}