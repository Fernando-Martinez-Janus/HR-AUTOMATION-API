using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>Validates the <see cref="CriticalityLevelInputModel"/> using FluentValidation rules.</summary>
    public class CriticalityLevelValidator : AbstractValidator<CriticalityLevelInputModel>
    {
        /// <summary>Defines validation rules for CriticalityLevelName, OrganizationId, and SortOrder.</summary>
        public CriticalityLevelValidator()
        {
            /// <summary>CriticalityLevelName must not be empty and must be between 2 and 200 characters.</summary>
            RuleFor(x => x.CriticalityLevelName)
                .NotEmpty()
                .WithErrorCode(Exceptions.CriticalityLevelNameLengthInvalid.ToString())
                .MinimumLength(2)
                .WithErrorCode(Exceptions.CriticalityLevelNameLengthInvalid.ToString())
                .MaximumLength(200)
                .WithErrorCode(Exceptions.CriticalityLevelNameLengthInvalid.ToString());

            /// <summary>OrganizationId must be greater than zero.</summary>
            //RuleFor(x => x.OrganizationId)
            //    .GreaterThan(0)
            //    .WithErrorCode(Exceptions.CriticalityLevelNameLengthInvalid.ToString());

            /// <summary>SortOrder must be greater than zero.</summary>
            RuleFor(x => x.SortOrder)
                .GreaterThan(0)
                .WithErrorCode(Exceptions.CriticalityLevelNameLengthInvalid.ToString());
        }
    }
}
