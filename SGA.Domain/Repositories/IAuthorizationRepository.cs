using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SGA.Domain.Entities.Authorizations;
using SGA.Domain.Repositories.Base;

namespace SGA.Domain.Repositories
{
    public interface IAuthorizationRepository : IRepository<Authorization, int>
    {
    }
}
