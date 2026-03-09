using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SGA.Domain.Entities.Transportation;
using SGA.Domain.Repositories.Base;

namespace SGA.Domain.Repositories
{
    public interface IRouteRepository : IRepository<Route, int>
    {
    }
}
