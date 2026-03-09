using Microsoft.EntityFrameworkCore;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;
using SGA.Persistence.Context;
using SGA.Persistence.Repositories;

namespace SGA.Persistence.Repositories.Users
{
    public class ModeRepository : Repository<Mode, int>, IModeRepository
    {
        public ModeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<Mode?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return Entities.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }
    }
}
