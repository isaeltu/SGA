using SGA.Domain.Entities.Incidents;
using SGA.Domain.Repositories;
using SGA.Persistence.Context;

namespace SGA.Persistence.Repositories
{
    public class IncidentRepository : Repository<Incident, int>, IIncidentRepository
    {
        public IncidentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
