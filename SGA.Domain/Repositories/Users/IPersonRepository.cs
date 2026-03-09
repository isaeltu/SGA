using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Base;

namespace SGA.Domain.Repositories.Users
{
    public interface IPersonRepository : IRepository<Person, int>
    {
        Task<Person?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<Person?> GetByCedulaAsync(string cedula, CancellationToken cancellationToken = default);
    }
}