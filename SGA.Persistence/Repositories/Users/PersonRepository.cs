using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGA.Domain.Entities.Users;
using SGA.Persistence.Context;

namespace SGA.Persistence.Repositories.Users
{
    public class PersonRepository
    {
        public readonly ApplicationDbContext _context;
        public readonly DbSet<Person> _entity;

        public PersonRepository(ApplicationDbContext context)
        {
            _context = context;
            _entity = context.Set<Person>();

        }


    }
}
