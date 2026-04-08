using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class UpdateBusCommandValidator : AbstractValidator<UpdateBusCommand>
    {
        public UpdateBusCommandValidator()
        {
            RuleFor(x => x.BusId).GreaterThan(0);
            RuleFor(x => x.InstitutionId).GreaterThan(0);
            RuleFor(x => x.LicensePlate).NotEmpty().MaximumLength(12);
            RuleFor(x => x.Model).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Year).InclusiveBetween(1990, 2100);
            RuleFor(x => x.Capacity).GreaterThan(0);
            RuleFor(x => x.ModifiedBy).NotEmpty().MaximumLength(255);
        }
    }
}
