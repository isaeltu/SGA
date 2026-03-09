using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class BoardReservationCommandValidator : AbstractValidator<BoardReservationCommand>
    {
        public BoardReservationCommandValidator()
        {
            RuleFor(x => x.ReservationId).GreaterThan(0);
            RuleFor(x => x.ModifiedBy).NotEmpty().MaximumLength(255);
        }
    }
}
