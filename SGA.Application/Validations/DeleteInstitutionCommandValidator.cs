using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class DeleteInstitutionCommandValidator : AbstractValidator<DeleteInstitutionCommand>
    {
        public DeleteInstitutionCommandValidator()
        {
            RuleFor(x => x.InstitutionId).GreaterThan(0);
        }
    }
}
