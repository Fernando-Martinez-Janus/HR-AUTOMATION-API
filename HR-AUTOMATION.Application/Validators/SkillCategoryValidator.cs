using FluentValidation;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    public class SkillCategoryValidator : AbstractValidator<SkillCategoryInputModel>
    {
        public SkillCategoryValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .WithErrorCode(Exceptions.SkillCategoryNameLengthInvalid.ToString())
                .MinimumLength(2)
                .WithErrorCode(Exceptions.SkillCategoryNameLengthInvalid.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.SkillCategoryNameLengthInvalid.ToString());

            RuleFor(x => x.OrganizationId)
                .GreaterThan(0)
                .WithErrorCode(Exceptions.InvalidOrganizationId.ToString());
        }
    }
}
