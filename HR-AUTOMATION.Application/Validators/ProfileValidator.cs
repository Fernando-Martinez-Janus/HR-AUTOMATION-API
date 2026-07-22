using FluentValidation;
using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Validators;

/// <summary>
/// Validates a <see cref="ProfileInputModel"/> before processing.
/// </summary>
public class ProfileValidator : AbstractValidator<ProfileInputModel>
{
    public ProfileValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotNull()
            .WithErrorCode(Exceptions.OrganizationRequired.ToString());

        RuleFor(x => x.AreaLevelId)
            .NotNull()
            .WithErrorCode(Exceptions.ProfileAreaLevelRequired.ToString());

        RuleFor(x => x.SeniorityLevelId)
            .NotNull()
            .WithErrorCode(Exceptions.ProfileSeniorityLevelRequired.ToString());

        RuleFor(x => x.ProfileName)
            .NotEmpty()
            .WithErrorCode(Exceptions.ProfileNameRequired.ToString())
            .MaximumLength(150)
            .WithErrorCode(Exceptions.ProfileNameLengthInvalid.ToString());

        RuleFor(x => x.ProfileDescription)
            .MaximumLength(1000)
            .WithErrorCode(Exceptions.ProfileDescriptionLengthInvalid.ToString())
            .When(x => x.ProfileDescription is not null);
    }
}
