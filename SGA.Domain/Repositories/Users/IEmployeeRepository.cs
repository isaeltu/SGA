using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Base;

namespace SGA.Domain.Repositories.Users
{
    public interface IEmployeeRepository : IRepository<Employee, int>
    {
    }
}
