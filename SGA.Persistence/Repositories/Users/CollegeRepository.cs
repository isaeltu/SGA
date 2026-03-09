using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;
using SGA.Persistence.Context;
using SGA.Persistence.Repositories;

namespace SGA.Persistence.Repositories.Users
{
    public class CollegeRepository : Repository<College, int>, ICollegeRepository
    {
        public CollegeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
