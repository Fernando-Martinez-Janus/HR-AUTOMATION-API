using FluentValidation;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>Validates the <see cref="SeniorityLevelInputModel"/> using FluentValidation rules.</summary>
    public class SeniorityLevelValidator : AbstractValidator<SeniorityLevelInputModel>
    {
        /// <summary>Defines validation rules for SeniorityName, OrganizationId, and SortOrder.</summary>
        public SeniorityLevelValidator()
        {
            /// <summary>SeniorityName must not be empty and must be between 2 and 200 characters.</summary>
            RuleFor(x => x.SeniorityName)
                .NotEmpty()
                .WithErrorCode(Exceptions.SeniorityNameLengthInvalid.ToString())
                .MinimumLength(2)
                .WithErrorCode(Exceptions.SeniorityNameLengthInvalid.ToString())
                .MaximumLength(200)
                .WithErrorCode(Exceptions.SeniorityNameLengthInvalid.ToString());

            /// <summary>OrganizationId must be greater than zero.</summary>
            RuleFor(x => x.OrganizationId)
                .GreaterThan(0)
                .WithErrorCode(Exceptions.InvalidOrganizationId.ToString());

            /// <summary>SortOrder must be greater than zero when provided.</summary>
            RuleFor(x => x.SortOrder)
                .GreaterThan(0)
                .When(x => x.SortOrder.HasValue)
                .WithErrorCode(Exceptions.InvalidSortOrder.ToString());
        }
    }
}
