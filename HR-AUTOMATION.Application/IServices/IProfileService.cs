using HR_AUTOMATION.Application.InputModels;

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




}
