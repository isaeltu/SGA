using Microsoft.EntityFrameworkCore;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;
using SGA.Persistence.Context;
using SGA.Persistence.Repositories;

namespace SGA.Persistence.Repositories.Users
{
    public class InstitutionRepository : Repository<Institution, int>, IInstitutionRepository
    {
        public InstitutionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<Institution?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return Entities.FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
        }
    }
}
