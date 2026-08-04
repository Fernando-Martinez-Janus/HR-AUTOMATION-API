using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    public class ScolarityLevelValidator : AbstractValidator<ScolarityLevelInputModel>
    {
        public ScolarityLevelValidator()
        {
            RuleFor(x => x.OrganizationId)
                .NotNull()
                .WithErrorCode(Exceptions.OrganizationRequired.ToString());

            RuleFor(x => x.LevelName)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.NameRequired.ToString());

            RuleFor(x => x.LevelDescription)
                .MaximumLength(300)
                .WithErrorCode(Exceptions.NameRequired.ToString());
        }
    }
}
