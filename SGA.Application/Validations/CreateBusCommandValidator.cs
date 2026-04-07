using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class CreateBusCommandValidator : AbstractValidator<CreateBusCommand>
    {
        public CreateBusCommandValidator()
        {
            RuleFor(x => x.InstitutionId).GreaterThan(0);
            RuleFor(x => x.LicensePlate).NotEmpty().MaximumLength(12);
            RuleFor(x => x.Model).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Year).InclusiveBetween(1990, 2100);
            RuleFor(x => x.Capacity).GreaterThan(0);
            RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(255);
        }
    }
}
