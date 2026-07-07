
using FluentValidation;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>
    /// Validates the <see cref="PermissionInputModel"/> using FluentValidation rules.
    /// </summary>
    public class PermissionValidator : AbstractValidator<PermissionInputModel>
    {
        /// <summary>
        /// Defines validation rules for PermissionName.
        /// </summary>
        public PermissionValidator()
        {
            /// <summary>PermissionName must not be empty and must be between 3 and 100 characters.</summary>
            RuleFor(x => x.PermissionName)
                .NotEmpty()
                .WithErrorCode(Exceptions.PermissionNameLengthInvalid.ToString())
                .MinimumLength(3)
                .WithErrorCode(Exceptions.PermissionNameLengthInvalid.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.PermissionNameLengthInvalid.ToString());
        }
    }
}
