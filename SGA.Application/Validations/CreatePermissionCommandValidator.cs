using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
    {
        public CreatePermissionCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(255);
            RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(255);
        }
    }
}