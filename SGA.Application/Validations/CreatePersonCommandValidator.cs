using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
    {
        public CreatePersonCommandValidator()
        {
            RuleFor(x => x.InstitutionId).GreaterThan(0);
            RuleFor(x => x.RoleId).GreaterThan(0);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(100);
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(15);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Cedula).NotEmpty().Length(11);
            RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(255);
        }
    }
}
