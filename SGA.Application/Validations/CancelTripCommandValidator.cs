using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class CancelTripCommandValidator : AbstractValidator<CancelTripCommand>
    {
        public CancelTripCommandValidator()
        {
            RuleFor(x => x.TripId).GreaterThan(0);
            RuleFor(x => x.ModifiedBy).NotEmpty().MaximumLength(255);
        }
    }
}
