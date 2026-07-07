using FluentValidation;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    public class VacancyValidator : AbstractValidator<VacancyInputModel>
    {
        public VacancyValidator()
        {
            RuleFor(x => x.VacancyTitle)
                .NotEmpty()
                .WithErrorCode(Exceptions.VacancyTitleLengthInvalid.ToString())
                .MinimumLength(3)
                .WithErrorCode(Exceptions.VacancyTitleLengthInvalid.ToString())
                .MaximumLength(200)
                .WithErrorCode(Exceptions.VacancyTitleLengthInvalid.ToString());

            RuleFor(x => x.PositionCount)
                .GreaterThanOrEqualTo(1)
                .WithErrorCode(Exceptions.InvalidPositionCount.ToString());
        }
    }
}
