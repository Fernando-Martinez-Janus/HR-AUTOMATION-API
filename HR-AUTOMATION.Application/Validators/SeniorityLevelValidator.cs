using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>
    /// Defines validation rules for the <see cref="SeniorityLevelInputModel"/> model.
    /// </summary>
    public class SeniorityLevelValidator : AbstractValidator<SeniorityLevelInputModel>
    {
        public SeniorityLevelValidator()
        {
            RuleFor(x => x.OrganizationId)
                .NotNull()
                .WithErrorCode(Exceptions.OrganizationRequired.ToString());

            RuleFor(x => x.SeniorityName)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.SeniorityLevelNameLengthInvalid.ToString());
        }
    }
}
