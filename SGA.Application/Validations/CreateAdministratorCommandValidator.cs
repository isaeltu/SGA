using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class CreateAdministratorCommandValidator : AbstractValidator<CreateAdministratorCommand>
    {
        public CreateAdministratorCommandValidator()
        {
            RuleFor(x => x.PersonId).GreaterThan(0);
            RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(255);
        }
    }
}
