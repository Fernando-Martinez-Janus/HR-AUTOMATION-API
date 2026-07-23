using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;
using Shared.Kernel.ViewModels;

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
    /// Retrieves profiles matching the specified search criteria.
    /// </summary>
    /// <param name="model">The search criteria.</param>
    /// <returns>A collection of matching profiles.</returns>
    Task<PaginationResponse<ProfileViewModel>> SearchAsync(ProfileSearchInputModel model);

    /// <summary>
    /// Retrieves a profile by its identifier.
    /// </summary>
    /// <param name="id">The profile identifier.</param>
    /// <returns>The profile information.</returns>
    Task<ProfileViewModel> GetAsync(int id);




    /// <summary>
    /// Updates an existing profile.
    /// </summary>
    /// <param name="id">The identifier of the profile to update.</param>
    /// <param name="model">The updated profile information.</param>
    Task<int> UpdateAsync(int id, ProfileInputModel model);

    /// <summary>
    /// Deletes an existing profile.
    /// </summary>
    /// <param name="id">The identifier of the profile to delete.</param>
    Task DeleteAsync(int id);

}
