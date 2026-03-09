using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class CancelReservationCommandValidator : AbstractValidator<CancelReservationCommand>
    {
        public CancelReservationCommandValidator()
        {
            RuleFor(x => x.ReservationId).GreaterThan(0);
            RuleFor(x => x.ModifiedBy).NotEmpty().MaximumLength(255);
        }
    }
}
