namespace HR_AUTOMATION.Application.ViewModels
{
    /// <summary>
    /// Represents the authentication data returned to the client after a successful login or token refresh.
    /// </summary>
    public class AuthViewModel
    {
        /// <summary>
        /// The JWT access token issued for the authenticated user.
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// The refresh token that can be used to obtain a new access token when the current one expires.
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}