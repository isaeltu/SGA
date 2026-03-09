using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Description).MaximumLength(255);
            RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(255);
        }
    }
}