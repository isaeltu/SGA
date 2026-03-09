using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class CreateOperatorCommandValidator : AbstractValidator<CreateOperatorCommand>
    {
        public CreateOperatorCommandValidator()
        {
            RuleFor(x => x.PersonId).GreaterThan(0);
            RuleFor(x => x.AssignedArea).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ShiftNumber).GreaterThan(0);
            RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(255);
        }
    }
}
