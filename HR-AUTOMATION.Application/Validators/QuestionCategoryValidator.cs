using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>
    /// Defines validation rules for the <see cref="QuestionCategoryInputModel"/> model.
    /// </summary>
    public class QuestionCategoryValidator : AbstractValidator<QuestionCategoryInputModel>
    {
        public QuestionCategoryValidator()
        {
            RuleFor(x => x.OrganizationId)
                .NotNull()
                .WithErrorCode(Exceptions.OrganizationRequired.ToString());

            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.QuestionCategoryNameLengthInvalid.ToString());

            RuleFor(x => x.CategoryDescription)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(300)
                .WithErrorCode(Exceptions.QuestionCategoryDescriptionLengthInvalid.ToString());
        }
    }
}
