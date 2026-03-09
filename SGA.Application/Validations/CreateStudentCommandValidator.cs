using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentCommandValidator()
        {
            RuleFor(x => x.PersonId).GreaterThan(0);
            RuleFor(x => x.CollegeId).GreaterThan(0);
            RuleFor(x => x.EnrollmentId).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Period).NotEmpty().MaximumLength(20);
            RuleFor(x => x.CareerName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(255);
        }
    }
}
