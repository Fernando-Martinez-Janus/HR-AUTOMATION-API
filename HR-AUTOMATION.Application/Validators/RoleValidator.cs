using FluentValidation;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    public class RoleValidator : AbstractValidator<RoleInputModel>
    {
        public RoleValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty()
                .WithErrorCode(Exceptions.RoleNameLengthInvalid.ToString())
                .MinimumLength(3)
                .WithErrorCode(Exceptions.RoleNameLengthInvalid.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.RoleNameLengthInvalid.ToString());

            RuleFor(x => x.PermissionIds)
                .NotEmpty()
                .WithErrorCode(Exceptions.RolePermissionsEmpty.ToString());
        }
    }
}
