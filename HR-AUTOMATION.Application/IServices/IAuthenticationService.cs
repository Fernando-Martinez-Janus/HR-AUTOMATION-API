using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Authenticates users and issues the application's own JWT, regardless of the
    /// identity source (Google Sign-In or email/password).
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Validates the Google ID token, authenticates the user against the application's database,
        /// and issues an application JWT access token.
        /// </summary>
        /// <param name="model">The Google Sign-In request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The application access token and authenticated user information.</returns>
        Task<AuthenticationResponseViewModel> LoginWithGoogleAsync(GoogleLoginInputModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates the email and password against the application's database
        /// and issues an application JWT access token.
        /// </summary>
        /// <param name="model">The email/password login request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The application access token and authenticated user information.</returns>
        Task<AuthenticationResponseViewModel> LoginWithEmailAsync(LoginInputModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates a refresh token, rotates it, and issues a new application JWT access token
        /// for the user it belongs to.
        /// </summary>
        /// <param name="model">The refresh token request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The application access token and authenticated user information.</returns>
        Task<AuthenticationResponseViewModel> RefreshAsync(RefreshTokenInputModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes a refresh token. Always succeeds, even if the token does not exist or was
        /// already revoked.
        /// </summary>
        /// <param name="model">The refresh token to revoke.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        Task LogoutAsync(RefreshTokenInputModel model, CancellationToken cancellationToken = default);
    }
}
