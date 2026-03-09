using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class UpdateModeCommandValidator : AbstractValidator<UpdateModeCommand>
    {
        public UpdateModeCommandValidator()
        {
            RuleFor(x => x.ModeId).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.ModifiedBy).NotEmpty().MaximumLength(255);
        }
    }
}
