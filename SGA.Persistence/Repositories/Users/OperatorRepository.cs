using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;
using SGA.Persistence.Context;
using SGA.Persistence.Repositories;

namespace SGA.Persistence.Repositories.Users
{
    public class OperatorRepository : Repository<Operator, int>, IOperatorRepository
    {
        public OperatorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
