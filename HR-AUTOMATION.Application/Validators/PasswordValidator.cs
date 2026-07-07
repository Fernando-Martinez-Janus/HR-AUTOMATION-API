using FluentValidation;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>
    /// Validates a password string using FluentValidation rules for complexity requirements.
    /// </summary>
    public class PasswordValidator : AbstractValidator<string?>
    {
        /// <summary>
        /// Defines validation rules: not empty, minimum 8 characters, maximum 126 characters,
        /// at least one uppercase letter, one lowercase letter, one digit, and one special character.
        /// </summary>
        public PasswordValidator()
        {
            /// <summary>
            /// Validates the password string.
            /// </summary>
            RuleFor(x => x)
                /// <summary>Must not be empty.</summary>
                .NotEmpty().WithMessage("Must not be empty.")
                /// <summary>Must have at least 8 characters.</summary>
                .MinimumLength(8).WithMessage("Must have at least 8 characters.")
                /// <summary>Must not exceed 126 characters.</summary>
                .MaximumLength(126).WithMessage("Must not exceed 126 characters.")
                /// <summary>Must contain at least one uppercase letter (Unicode letters included).</summary>
                .Matches(@"\p{Lu}").WithMessage("Must contain at least one uppercase letter (Unicode letters included).")
                /// <summary>Must contain at least one lowercase letter (Unicode letters included).</summary>
                .Matches(@"\p{Ll}").WithMessage("Must contain at least one lowercase letter (Unicode letters included).")
                /// <summary>Must contain at least one number.</summary>
                .Matches(@"\p{Nd}").WithMessage("Must contain at least one number.")
                /// <summary>Must contain at least one special character (non-letter and non-digit).</summary>
                .Matches(@"[^\p{L}\p{Nd}]").WithMessage("Must contain at least one special character (non-letter and non-digit).");
        }


    }
}


