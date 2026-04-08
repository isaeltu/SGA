using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using SGA.Persistence.Context;
using SGA.Persistence.Repositories;

namespace SGA.Persistence.Repositories.Users
{
    public class DriverRepository : Repository<Driver, int>, IDriverRepository
    {
        private readonly ApplicationDbContext _context;

        public DriverRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Driver?> GetByPersonIdAsync(int personId, CancellationToken cancellationToken = default)
        {
            return _context.Drivers.FirstOrDefaultAsync(x => x.PersonId == personId && !x.IsDeleted, cancellationToken);
        }
    }
}
