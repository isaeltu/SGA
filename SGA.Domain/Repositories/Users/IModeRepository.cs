using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Base;

namespace SGA.Domain.Repositories.Users
{
    public interface IModeRepository : IRepository<Mode, int>
    {
        Task<Mode?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}