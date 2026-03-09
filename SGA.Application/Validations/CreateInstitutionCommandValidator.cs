using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public class CreateInstitutionCommandValidator : AbstractValidator<CreateInstitutionCommand>
    {
        public CreateInstitutionCommandValidator()
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(255);
        }
    }
}
