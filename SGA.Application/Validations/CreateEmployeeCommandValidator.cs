using FluentValidation;
using SGA.Application.Commands;

namespace SGA.Application.Validations
{
    public sealed class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.PersonId).GreaterThan(0);
            RuleFor(x => x.DepartmentId).GreaterThan(0);
            RuleFor(x => x.EmployeeCode).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Position).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(255);
        }
    }
}
