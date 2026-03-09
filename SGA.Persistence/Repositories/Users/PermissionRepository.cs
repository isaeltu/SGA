using Microsoft.EntityFrameworkCore;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;
using SGA.Persistence.Context;
using SGA.Persistence.Repositories;

namespace SGA.Persistence.Repositories.Users
{
    public class PermissionRepository : Repository<Permission, int>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return Entities.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }
    }
}
