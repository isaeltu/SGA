using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class UpdateRouteCommandValidator : AbstractValidator<UpdateRouteCommand>
    {
        public UpdateRouteCommandValidator()
        {
            RuleFor(x => x.RouteId).GreaterThan(0);
            RuleFor(x => x.InstitutionId).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Origin).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Destination).NotEmpty().MaximumLength(120);
            RuleFor(x => x.DistanceKm).GreaterThan(0);
            RuleFor(x => x.EstimatedDurationMinutes).GreaterThan(0);
            RuleFor(x => x.ModifiedBy).NotEmpty().MaximumLength(255);
        }
    }
}