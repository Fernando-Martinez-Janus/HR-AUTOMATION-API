using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user with the provided login credentials.
        /// </summary>
        /// <param name="model">The login input model containing username and password.</param>
        /// <returns>Generated authentication token and refresh token.</returns>
        Task<AuthViewModel> LoginAsync(LoginInputModel model);

        /// <summary>
        /// Refreshes the authentication token for the current user.
        /// </summary>
        /// <param name="model">Current refresh token.</param>
        /// <returns>Generated authentication token and refresh token.</returns>
        Task<AuthViewModel> RefreshTokenAsync(RefreshTokenInputModel model);
    }
}
