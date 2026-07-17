using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    public class VacancyValidator : AbstractValidator<VacancyInputModel>
    {
        public VacancyValidator()
        {
            RuleFor(x => x.OrganizationId)
               .NotNull()
               .WithErrorCode(Exceptions.OrganizationRequired.ToString());

            RuleFor(x => x.VacancyTitle)
                .NotEmpty()
                .WithErrorCode(Exceptions.VacancyTitleRequired.ToString())
                .MaximumLength(200)
                .WithErrorCode(Exceptions.VacancyTitleLengthInvalid.ToString());

            RuleFor(x => x.PositionCount)
                .GreaterThan(0)
                .WithErrorCode(Exceptions.VacancyPositionCountInvalid.ToString());
        }
    }
}
