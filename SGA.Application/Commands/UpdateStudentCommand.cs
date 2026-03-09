using MediatR;

namespace SGA.Application.Commands
{
    public record UpdateStudentCommand(
        int studentId,
        string firstName,
        string lastName,
        string email,
        string period,
        string CareerName,
        string PhoneNumber,
        int collegeId
        ) : IRequest;
    
}
