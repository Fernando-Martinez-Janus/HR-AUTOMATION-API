using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    public class SearchRequestValidator : AbstractValidator<SearchRequestInputModel>
    {
        public SearchRequestValidator()
        {
            RuleFor(x => x.OrganizationId)
                .NotNull()
                .WithErrorCode(Exceptions.OrganizationRequired.ToString());

            RuleFor(x => x.VacancyId)
                .NotEmpty()
                .WithErrorCode(Exceptions.OrganizationRequired.ToString());
        }
    }
}
