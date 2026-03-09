using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;
using SGA.Persistence.Context;
using SGA.Persistence.Repositories;

namespace SGA.Persistence.Repositories.Users
{
    public class AdministratorRepository : Repository<Administrator, int>, IAdministratorRepository
    {
        public AdministratorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
