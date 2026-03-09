using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public class CreateModeCommandValidator : AbstractValidator<CreateModeCommand>
    {
        public CreateModeCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(255);
        }
    }
}