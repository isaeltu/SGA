using SGA.Domain.Entities.Authorizations;
using SGA.Domain.Repositories;
using SGA.Persistence.Context;

namespace SGA.Persistence.Repositories
{
    public class AuthorizationRepository : Repository<Authorization, int>, IAuthorizationRepository
    {
        public AuthorizationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
