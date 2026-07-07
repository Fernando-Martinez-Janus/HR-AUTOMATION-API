using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    public class LoginValidator : AbstractValidator<LoginInputModel>
    {

        public LoginValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Must not be empty.")
                .EmailAddress()
                .WithErrorCode(Exceptions.InvalidCredentials.ToString());



            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Must not be empty.");


        }
    }
}
