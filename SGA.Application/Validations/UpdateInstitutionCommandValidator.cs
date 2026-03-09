using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class UpdateInstitutionCommandValidator : AbstractValidator<UpdateInstitutionCommand>
    {
        public UpdateInstitutionCommandValidator()
        {
            RuleFor(x => x.InstitutionId).GreaterThan(0);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.ModifiedBy).NotEmpty().MaximumLength(255);
        }
    }
}
