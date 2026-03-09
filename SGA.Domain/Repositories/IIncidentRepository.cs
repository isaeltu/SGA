using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SGA.Domain.Entities.Incidents;
using SGA.Domain.Repositories.Base;

namespace SGA.Domain.Repositories
{
    public interface IIncidentRepository : IRepository<Incident, int>
    {
    }
}
