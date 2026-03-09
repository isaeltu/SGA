using SGA.Domain.Entities.Users;
using SGA.Domain.Common;
using SGA.Persistence.Repositories;
using SGA.Domain.Repositories.Users;
using SGA.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using SGA.Domain.Enums.Users;

namespace SGA.Persistence.Repositories.Users
{
    public class StudentRepository : Repository<Student, int>, IStudentRepository
    {
        public readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context) : base(context)
        
        {
            _context = context;
        }

        public async Task<Result<Student>>  GetByEnrollmentId(string enrollmentId, Person person)
        {
            var student = await _context.Students
                .Include(s => s.College)
                .Include(s => s.Person)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                 s => s.EnrollmentId.Value == enrollmentId &&
                 !s.IsDeleted &&
                 s.Person.Status == UserStatus.Active
                );


            
            
            if(student == null)
            {
                    return Result<Student>.Failure(new Error("Student.NotFound", "El estudiante no existe."));
            }
            
            return  Result<Student>.Success(student);

        }



        
    }
}
