using FluentValidation;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>Validates the <see cref="ProfileInputModel"/> using FluentValidation rules.</summary>
    public class ProfileValidator : AbstractValidator<ProfileInputModel>
    {
        /// <summary>Defines validation rules for ProfileName, OrganizationId, and SortOrder.</summary>
        public ProfileValidator()
        {
            /// <summary>ProfileName must not be empty and must be between 2 and 200 characters.</summary>
            RuleFor(x => x.ProfileName)
                .NotEmpty()
                .WithErrorCode(Exceptions.ProfileNameLengthInvalid.ToString())
                .MinimumLength(2)
                .WithErrorCode(Exceptions.ProfileNameLengthInvalid.ToString())
                .MaximumLength(200)
                .WithErrorCode(Exceptions.ProfileNameLengthInvalid.ToString());

            /// <summary>OrganizationId must be greater than zero.</summary>
            //RuleFor(x => x.OrganizationId)
            //    .GreaterThan(0)
            //    .WithErrorCode(Exceptions.ProfileNameLengthInvalid.ToString());

            /// <summary>SortOrder must be greater than zero.</summary>
            RuleFor(x => x.SortOrder)
                .GreaterThan(0)
                .WithErrorCode(Exceptions.ProfileNameLengthInvalid.ToString());
        }
    }
}
