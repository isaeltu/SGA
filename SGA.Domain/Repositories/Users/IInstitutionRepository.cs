using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Base;

namespace SGA.Domain.Repositories.Users
{
    public interface IInstitutionRepository : IRepository<Institution, int>
    {
        Task<Institution?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    }
}