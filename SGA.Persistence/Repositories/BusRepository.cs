using SGA.Domain.Entities.Transportation;
using SGA.Domain.Repositories;
using SGA.Persistence.Context;

namespace SGA.Persistence.Repositories
{
    public class BusRepository : Repository<Bus, int>, IBusRepository
    {
        public BusRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
