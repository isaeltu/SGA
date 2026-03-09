using SGA.Domain.Entities.Transportation;
using SGA.Domain.Repositories;
using SGA.Persistence.Context;

namespace SGA.Persistence.Repositories
{
    public class RouteRepository : Repository<Route, int>, IRouteRepository
    {
        public RouteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
