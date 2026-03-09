using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
    {
        public UpdatePermissionCommandValidator()
        {
            RuleFor(x => x.PermissionId).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(255);
            RuleFor(x => x.ModifiedBy).NotEmpty().MaximumLength(255);
        }
    }
}
