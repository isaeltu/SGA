using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Base;

namespace SGA.Domain.Repositories.Users
{
    public interface IRoleRepository : IRepository<Role, byte>
    {
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}