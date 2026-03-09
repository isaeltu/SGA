using SGA.Domain.Entities.Trips;
using SGA.Domain.Repositories;
using SGA.Persistence.Context;

namespace SGA.Persistence.Repositories
{
    public class TripRepository : Repository<Trip, int>, ITripRepository
    {
        public TripRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
