using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;
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

        public Task<Person?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            return _context.Persons.FirstOrDefaultAsync(
                x => EF.Property<string>(x, nameof(Person.Email)) == normalizedEmail,
                cancellationToken);
        }

        public Task<Person?> GetByCedulaAsync(string cedula, CancellationToken cancellationToken = default)
        {
            return _context.Persons.FirstOrDefaultAsync(x => x.Cedula == cedula, cancellationToken);
        }
    }
}
