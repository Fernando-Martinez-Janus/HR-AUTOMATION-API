namespace HR_AUTOMATION.Application.ViewModels
{
    /// <summary>
    /// Represents the response returned after a successful authentication, regardless of
    /// whether the user signed in with Google or with an email/password.
    /// </summary>
    public class AuthenticationResponseViewModel
    {
        /// <summary>
        /// Gets or sets the application's own JWT access token.
        /// </summary>
        public string AccessToken { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of seconds until the access token expires.
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the token type used in the Authorization header.
        /// </summary>
        public string TokenType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the authenticated user's information.
        /// </summary>
        public AuthenticatedUserViewModel User { get; set; } = null!;
    }
}
