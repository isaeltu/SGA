using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Base;

namespace SGA.Domain.Repositories.Users
{
    public interface IDriverRepository : IRepository<Driver, int>
    {
        Task<Driver?> GetByPersonIdAsync(int personId, CancellationToken cancellationToken = default);
    }
}
