using SGA.Domain.Common;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Base;

namespace SGA.Domain.Repositories.Users
{
    public interface IStudentRepository: IRepository<Student, int>
    {
       public Task<Result<Student>> GetByEnrollmentId(string enrollmentId, Person person);

    }
}
