using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class CreateDriverCommandValidator : AbstractValidator<CreateDriverCommand>
    {
        public CreateDriverCommandValidator()
        {
            RuleFor(x => x.PersonId).GreaterThan(0);
            RuleFor(x => x.DriverLicense).NotEmpty().MaximumLength(15);
            RuleFor(x => x.LicenseExpirationDate).GreaterThan(DateTimeOffset.UtcNow);
            RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(255);
        }
    }
}
