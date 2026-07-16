using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>
    /// Defines validation rules for the <see cref="SkillCategoryInputModel"/> model.
    /// </summary>
    public class SkillCategoryValidator : AbstractValidator<SkillCategoryInputModel>
    {
        public SkillCategoryValidator()
        {
            RuleFor(x => x.OrganizationId)
                .NotNull()
                .WithErrorCode(Exceptions.OrganizationRequired.ToString());

            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.SkillCategoryNameLengthInvalid.ToString());

            RuleFor(x => x.IconName)
                .MaximumLength(50)
                .WithErrorCode(Exceptions.SkillCategoryIconNameLengthInvalid.ToString())
                .When(x => !string.IsNullOrWhiteSpace(x.IconName));
        }
    }
}