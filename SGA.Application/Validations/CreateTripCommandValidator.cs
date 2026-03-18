using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class CreateTripCommandValidator : AbstractValidator<CreateTripCommand>
    {
        public CreateTripCommandValidator()
        {
            RuleFor(x => x.RouteId).GreaterThan(0);
            RuleFor(x => x.DriverId).GreaterThan(0);
            RuleFor(x => x.BusId).GreaterThan(0);
            RuleFor(x => x.ScheduledDepartureTime)
                .LessThan(x => x.ScheduledArrivalTime)
                .WithMessage("ScheduledDepartureTime must be before ScheduledArrivalTime.");
            RuleFor(x => x.AvailableSeats)
                .GreaterThan(0)
                .When(x => x.AvailableSeats.HasValue);
            RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(255);
        }
    }
}
