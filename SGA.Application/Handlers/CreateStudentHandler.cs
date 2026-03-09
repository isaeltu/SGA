using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;
using SGA.Domain.ValueObjects.Users;

namespace SGA.Application.Handlers
{
    public sealed class CreateStudentHandler : IRequestHandler<CreateStudentCommand, int>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IPersonRepository _personRepository;

        public CreateStudentHandler(IStudentRepository studentRepository, IPersonRepository personRepository)
        {
            _studentRepository = studentRepository;
            _personRepository = personRepository;
        }

        public async Task<int> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var person = await _personRepository.GetByIdAsync(request.PersonId, cancellationToken).ConfigureAwait(false);
            if (person is null)
            {
                throw new KeyNotFoundException($"Person with id {request.PersonId} was not found.");
            }

            var student = new Student(request.PersonId, request.CollegeId, request.Period, request.CareerName)
            {
                CollegeId = request.CollegeId,
                EnrollmentId = new EnrollmentId(request.EnrollmentId)
            };

            student.SetCreationInfo(request.CreatedBy);
            await _studentRepository.AddAsync(student, cancellationToken).ConfigureAwait(false);
            return student.Id;
        }
    }
}
