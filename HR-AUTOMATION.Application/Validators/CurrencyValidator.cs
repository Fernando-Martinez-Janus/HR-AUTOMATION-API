using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>
    /// Defines validation rules for the <see cref="CurrencyInputModel"/> model.
    /// </summary>
    public class CurrencyValidator : AbstractValidator<CurrencyInputModel>
    {
        public CurrencyValidator()
        {
            RuleFor(x => x.OrganizationId)
                .NotNull()
                .WithErrorCode(Exceptions.OrganizationRequired.ToString());

            RuleFor(x => x.CurrencyCode)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(3)
                .WithErrorCode(Exceptions.CurrencyCodeLengthInvalid.ToString());

            RuleFor(x => x.CurrencyName)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.CurrencyNameLengthInvalid.ToString());

            RuleFor(x => x.CurrencySymbol)
                .MaximumLength(5)
                .WithErrorCode(Exceptions.CurrencySymbolLengthInvalid.ToString())
                .When(x => !string.IsNullOrWhiteSpace(x.CurrencySymbol));
        }
    }
}
