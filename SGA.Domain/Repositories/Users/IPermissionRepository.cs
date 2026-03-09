using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Base;

namespace SGA.Domain.Repositories.Users
{
    public interface IPermissionRepository : IRepository<Permission, int>
    {
        Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}