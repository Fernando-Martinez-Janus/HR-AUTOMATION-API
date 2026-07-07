using FluentValidation;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    public class VacancyStatusValidator : AbstractValidator<VacancyStatusInputModel>
    {
        public VacancyStatusValidator()
        {
            RuleFor(x => x.StatusName)
                .NotEmpty()
                .WithErrorCode(Exceptions.VacancyStatusNameLengthInvalid.ToString())
                .MinimumLength(2)
                .WithErrorCode(Exceptions.VacancyStatusNameLengthInvalid.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.VacancyStatusNameLengthInvalid.ToString());

            RuleFor(x => x.StatusDescription)
                .NotEmpty()
                .WithErrorCode(Exceptions.VacancyStatusDescriptionInvalid.ToString())
                .MaximumLength(300)
                .WithErrorCode(Exceptions.VacancyStatusDescriptionInvalid.ToString());

            RuleFor(x => x.OrganizationId)
                .GreaterThan(0)
                .WithErrorCode(Exceptions.InvalidOrganizationId.ToString());
        }
    }
}
