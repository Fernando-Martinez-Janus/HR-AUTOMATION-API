using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Retrieves user profile information (role, organization, permissions). This service is
    /// responsible ONLY for profile retrieval, never for authentication.
    /// </summary>
    public interface IUserProfileService
    {
        /// <summary>
        /// Retrieves the profile of the currently authenticated user, identified exclusively
        /// from the JWT claims of the current request.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The authenticated user's profile.</returns>
        Task<UserProfileViewModel> GetMyProfileAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the profile of any user by identifier. Intended for administrator-only access.
        /// </summary>
        /// <param name="userId">The identifier of the user to load.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The requested user's profile.</returns>
        Task<UserProfileViewModel> GetByIdAsync(int userId, CancellationToken cancellationToken = default);
    }
}
