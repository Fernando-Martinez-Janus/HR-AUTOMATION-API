using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices;

/// <summary>
/// Defines the contract for profile management operations.
/// </summary>
public interface IProfileService
{
    /// <summary>
    /// Creates a new profile with its associated skills.
    /// </summary>
    /// <param name="model">The profile information.</param>
    /// <returns>The identifier of the newly created profile.</returns>
    Task<int> CreateAsync(ProfileInputModel model);


    /// <summary>
    /// Retrieves a profile by its identifier.
    /// </summary>
    /// <param name="id">The profile identifier.</param>
    /// <returns>The <see cref="ProfileViewModel"/> if found; otherwise throws <see cref="ResponseExceptionFactory"/>.</returns>
    Task<ProfileViewModel> GetAsync(int id);


}
