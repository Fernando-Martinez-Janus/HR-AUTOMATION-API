using FluentValidation;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    public class SkillValidator : AbstractValidator<SkillInputModel>
    {
        public SkillValidator()
        {
            RuleFor(x => x.SkillName)
                .NotEmpty()
                .WithErrorCode(Exceptions.SkillNameLengthInvalid.ToString())
                .MinimumLength(2)
                .WithErrorCode(Exceptions.SkillNameLengthInvalid.ToString())
                .MaximumLength(150)
                .WithErrorCode(Exceptions.SkillNameLengthInvalid.ToString());

            RuleFor(x => x.OrganizationId)
                .GreaterThan(0)
                .WithErrorCode(Exceptions.InvalidOrganizationId.ToString());

            RuleFor(x => x.SkillCategoryId)
                .GreaterThan(0)
                .WithErrorCode(Exceptions.InvalidSkillCategoryId.ToString());
        }
    }
}
