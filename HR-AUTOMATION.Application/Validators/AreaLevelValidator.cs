using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>Validates the <see cref="AreaLevelInputModel"/> using FluentValidation rules.</summary>
    public class AreaLevelValidator : AbstractValidator<AreaLevelInputModel>
    {
        /// <summary>Defines validation rules for AreaLevelName, OrganizationId, and SortOrder.</summary>
        public AreaLevelValidator()
        {
            /// <summary>AreaLevelName must not be empty and must be between 2 and 200 characters.</summary>
            RuleFor(x => x.AreaLevelName)
                .NotEmpty()
                .WithErrorCode(Exceptions.AreaLevelNameLengthInvalid.ToString())
                .MinimumLength(2)
                .WithErrorCode(Exceptions.AreaLevelNameLengthInvalid.ToString())
                .MaximumLength(200)
                .WithErrorCode(Exceptions.AreaLevelNameLengthInvalid.ToString());

            /// <summary>OrganizationId must be greater than zero.</summary>
            //RuleFor(x => x.OrganizationId)
            //    .GreaterThan(0)
            //    .WithErrorCode(Exceptions.AreaLevelNameLengthInvalid.ToString());

            /// <summary>SortOrder must be greater than zero.</summary>
            RuleFor(x => x.SortOrder)
                .GreaterThan(0)
                .WithErrorCode(Exceptions.AreaLevelNameLengthInvalid.ToString());
        }
    }
}
