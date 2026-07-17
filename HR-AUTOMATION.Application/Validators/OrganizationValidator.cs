using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>
    /// Defines validation rules for the <see cref="OrganizationInputModel"/> model.
    /// </summary>
    public class OrganizationValidator : AbstractValidator<OrganizationInputModel>
    {
        public OrganizationValidator()
        {
            RuleFor(x => x.OrganizationName)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(150)
                .WithErrorCode(Exceptions.OrganizationNameLengthInvalid.ToString());

            RuleFor(x => x.Slug)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(50)
                .WithErrorCode(Exceptions.OrganizationSlugLengthInvalid.ToString());
        }
    }
}
