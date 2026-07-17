using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>
    /// Defines validation rules for the <see cref="EmploymentTypeInputModel"/> model.
    /// </summary>
    public class EmploymentTypeValidator : AbstractValidator<EmploymentTypeInputModel>
    {
        public EmploymentTypeValidator()
        {
            RuleFor(x => x.OrganizationId)
                .NotNull()
                .WithErrorCode(Exceptions.OrganizationRequired.ToString());

            RuleFor(x => x.TypeName)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.EmploymentTypeNameLengthInvalid.ToString());
        }
    }
}
