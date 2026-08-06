using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    public class SearchResultsValidator : AbstractValidator<SearchResultsInputModel>
    {
        public SearchResultsValidator()
        {
            RuleFor(x => x.OrganizationId)
                .NotNull()
                .WithErrorCode(Exceptions.OrganizationRequired.ToString());
            RuleFor(x => x.SearchRequestId)
                .NotNull()
                .WithErrorCode(Exceptions.SearchRequestRequired.ToString());
        }
    }
}
