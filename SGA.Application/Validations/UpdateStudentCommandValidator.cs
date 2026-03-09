using System.Security.AccessControl;
using FluentValidation;
using SGA.Application.Commands;
namespace SGA.Application.Validations
{
    public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentCommandValidator()
        {
            RuleFor(x => x.firstName)
                .NotEmpty()
                .NotNull()
                .MaximumLength(40);

            RuleFor(x => x.lastName)
                .NotEmpty()
                .NotNull()
                .MaximumLength(40);

            RuleFor(x => x.email)
                .NotEmpty();

            
        }
    }
}
