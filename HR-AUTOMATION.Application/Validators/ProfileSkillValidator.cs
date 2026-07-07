using FluentValidation;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    public class ProfileSkillValidator : AbstractValidator<ProfileSkillInputModel>
    {
        public ProfileSkillValidator()
        {
            RuleFor(x => x.ProfileId)
                .GreaterThan(0)
                .WithErrorCode(Exceptions.InvalidProfileId.ToString());

            RuleFor(x => x.SkillId)
                .GreaterThan(0)
                .WithErrorCode(Exceptions.InvalidSkillId.ToString());

            RuleFor(x => x.SkillLevelId)
                .GreaterThan(0)
                .WithErrorCode(Exceptions.InvalidSkillLevelId.ToString());

            RuleFor(x => x.SkillWeight)
                .NotEmpty()
                .WithErrorCode(Exceptions.InvalidSkillWeight.ToString())
                .Must(weight => weight == "obligatorio" || weight == "deseable")
                .WithErrorCode(Exceptions.InvalidSkillWeight.ToString());
        }
    }
}
