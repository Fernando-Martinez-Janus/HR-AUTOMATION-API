using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>
    /// Defines validation rules for the <see cref="RejectionReasonInputModel"/> model.
    /// </summary>
    public class RejectionReasonValidator : AbstractValidator<RejectionReasonInputModel>
    {
        public RejectionReasonValidator()
        {
            RuleFor(x => x.OrganizationId)
                .NotNull()
                .WithErrorCode(Exceptions.OrganizationRequired.ToString());

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(300)
                .WithErrorCode(Exceptions.RejectionReasonDescriptionLengthInvalid.ToString());
        }
    }
}
