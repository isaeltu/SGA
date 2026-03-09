using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class StartTripCommandValidator : AbstractValidator<StartTripCommand>
    {
        public StartTripCommandValidator()
        {
            RuleFor(x => x.TripId).GreaterThan(0);
            RuleFor(x => x.ModifiedBy).NotEmpty().MaximumLength(255);
        }
    }
}
