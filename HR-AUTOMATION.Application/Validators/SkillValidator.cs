using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>
    /// Defines validation rules for the <see cref="SkillInputModel"/> model.
    /// </summary>
    public class SkillValidator : AbstractValidator<SkillInputModel>
    {
        public SkillValidator()
        {
            RuleFor(x => x.OrganizationId)
               .NotNull()
               .WithErrorCode(Exceptions.OrganizationRequired.ToString());

            RuleFor(x => x.SkillName)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.SkillNameLengthInvalid.ToString());
        }
    }
}
