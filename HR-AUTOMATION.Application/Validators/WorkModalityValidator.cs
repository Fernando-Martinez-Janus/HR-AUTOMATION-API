using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators
{
    /// <summary>
    /// Defines validation rules for the <see cref="WorkModalityInputModel"/> model.
    /// </summary>
    public class WorkModalityValidator : AbstractValidator<WorkModalityInputModel>
    {
        public WorkModalityValidator()
        {
            RuleFor(x => x.OrganizationId)
                .NotNull()
                .WithErrorCode(Exceptions.OrganizationRequired.ToString());

            RuleFor(x => x.ModalityName)
                .NotEmpty()
                .WithErrorCode(Exceptions.NameRequired.ToString())
                .MaximumLength(100)
                .WithErrorCode(Exceptions.WorkModalityNameLengthInvalid.ToString());
        }
    }
}
