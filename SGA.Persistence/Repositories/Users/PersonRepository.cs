using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;
using SGA.Domain.ValueObjects.Users;
using SGA.Persistence.Context;
using SGA.Persistence.Repositories;

namespace SGA.Persistence.Repositories.Users
{
    public class PersonRepository : Repository<Person, int>, IPersonRepository
    {
        public readonly ApplicationDbContext _context;

        public PersonRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<bool> ExistsByIdAsync(int personId, CancellationToken cancellationToken = default)
        {
            return _context.Persons.AsNoTracking().AnyAsync(x => x.Id == personId, cancellationToken);
        }

        public Task<Person?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            var emailValueObject = new Email(normalizedEmail);

            return _context.Persons.FirstOrDefaultAsync(
                x => x.Email == emailValueObject,
                cancellationToken);
        }

        public Task<Person?> GetByCedulaAsync(string cedula, CancellationToken cancellationToken = default)
        {
            return _context.Persons.FirstOrDefaultAsync(x => x.Cedula == cedula, cancellationToken);
        }

        public async Task<DriverPersonLookup?> GetDriverLookupByPersonIdAsync(int personId, CancellationToken cancellationToken = default)
        {
            var data = await _context.Persons
                .AsNoTracking()
                .Where(p => p.Id == personId)
                .Select(p => new
                {
                    p.Id,
                    p.InstitutionId,
                    p.FirstName,
                    p.LastName,
                    p.IsDeleted
                })
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            if (data is null)
            {
                return null;
            }

            var fullName = $"{data.FirstName} {data.LastName}".Trim();
            return new DriverPersonLookup(
                data.Id,
                data.InstitutionId,
                string.IsNullOrWhiteSpace(fullName) ? $"Driver {data.Id}" : fullName,
                data.IsDeleted);
        }
    }
}
