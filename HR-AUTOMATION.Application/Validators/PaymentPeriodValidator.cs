using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>
    /// Defines validation rules for the <see cref="PaymentPeriodInputModel"/> model.
    /// </summary>
    public class PaymentPeriodValidator : AbstractValidator<PaymentPeriodInputModel>
    {
        public PaymentPeriodValidator()
        {
            RuleFor(x => x.OrganizationId)
                .NotNull()
                .WithErrorCode(Exceptions.OrganizationRequired.ToString());

            RuleFor(x => x.PeriodName)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.PaymentPeriodNameLengthInvalid.ToString());
        }
    }
}
