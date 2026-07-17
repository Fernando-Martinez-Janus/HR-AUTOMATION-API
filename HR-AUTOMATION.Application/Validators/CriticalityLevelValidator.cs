using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>
    /// Defines validation rules for the <see cref="CriticalityLevelInputModel"/> model.
    /// </summary>
    public class CriticalityLevelValidator : AbstractValidator<CriticalityLevelInputModel>
    {
        public CriticalityLevelValidator()
        {
            RuleFor(x => x.OrganizationId)
                .NotNull()
                .WithErrorCode(Exceptions.OrganizationRequired.ToString());

            RuleFor(x => x.LevelName)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.CriticalityLevelNameLengthInvalid.ToString());

            RuleFor(x => x.LevelDescription)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(300)
                .WithErrorCode(Exceptions.CriticalityLevelDescriptionLengthInvalid.ToString());
        }
    }
}
