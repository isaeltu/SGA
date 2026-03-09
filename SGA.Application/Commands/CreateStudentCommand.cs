using MediatR;

namespace SGA.Application.Commands
{
    public sealed record CreateStudentCommand(
        int PersonId,
        int CollegeId,
        string EnrollmentId,
        string Period,
        string CareerName,
        string CreatedBy) : IRequest<int>;
}
