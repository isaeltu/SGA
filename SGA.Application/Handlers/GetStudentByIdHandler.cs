using MediatR;
using SGA.Application.DTOs.Users;
using SGA.Application.Queries;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class GetStudentByIdHandler : IRequestHandler<GetStudentByIdQuery, StudentDto?>
    {
        private readonly IStudentRepository _studentRepository;

        public GetStudentByIdHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<StudentDto?> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken).ConfigureAwait(false);
            if (student is null || student.Person is null)
            {
                return null;
            }

            var person = new PersonDto(
                student.Person.Id,
                student.Person.InstitutionId,
                student.Person.Email.Value,
                student.Person.FirstName ?? string.Empty,
                student.Person.LastName ?? string.Empty,
                student.Person.Cedula,
                student.Person.IsActive);

            return new StudentDto(
                student.Id,
                student.PersonId,
                student.EnrollmentId.Value,
                student.CareerName,
                student.Period,
                person);
        }
    }
}
