using FluentValidation;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    public class UserValidator : AbstractValidator<UserInputModel>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithErrorCode(Exceptions.InvalidEmail.ToString());

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithErrorCode(Exceptions.UsernameLengthInvalid.ToString())
                .MinimumLength(3)
                .WithErrorCode(Exceptions.UsernameLengthInvalid.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.UsernameLengthInvalid.ToString());

            RuleFor(x => x.Password)
                .SetValidator(new PasswordValidator())
                .When(x => !string.IsNullOrEmpty(x.Password));
        }
    }
}
