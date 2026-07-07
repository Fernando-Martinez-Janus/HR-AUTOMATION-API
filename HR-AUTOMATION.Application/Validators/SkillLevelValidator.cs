using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>Validates the <see cref="SkillLevelInputModel"/> using FluentValidation rules.</summary>
    public class SkillLevelValidator : AbstractValidator<SkillLevelInputModel>
    {
        /// <summary>Defines validation rules for SkillLevelName, OrganizationId, and SortOrder.</summary>
        public SkillLevelValidator()
        {
            /// <summary>SkillLevelName must not be empty and must be between 2 and 200 characters.</summary>
            RuleFor(x => x.SkillLevelName)
                .NotEmpty()
                .WithErrorCode(Exceptions.SkillLevelNameLengthInvalid.ToString())
                .MinimumLength(2)
                .WithErrorCode(Exceptions.SkillLevelNameLengthInvalid.ToString())
                .MaximumLength(200)
                .WithErrorCode(Exceptions.SkillLevelNameLengthInvalid.ToString());

            /// <summary>OrganizationId must be greater than zero.</summary>
            //RuleFor(x => x.OrganizationId)
            //    .GreaterThan(0)
            //    .WithErrorCode(Exceptions.SkillLevelNameLengthInvalid.ToString());

            /// <summary>SortOrder must be greater than zero.</summary>
            RuleFor(x => x.SortOrder)
                .GreaterThan(0)
                .WithErrorCode(Exceptions.SkillLevelNameLengthInvalid.ToString());
        }
    }
}
