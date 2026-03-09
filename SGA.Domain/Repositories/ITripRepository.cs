using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SGA.Domain.Entities.Trips;
using SGA.Domain.Repositories.Base;

namespace SGA.Domain.Repositories
{
    public interface ITripRepository : IRepository<Trip, int>
    {
    }
}
