namespace HR_AUTOMATION.Application.ViewModels
{
    /// <summary>
    /// Represents the tokens returned after a successful authentication, regardless of whether
    /// the user signed in with Google, email/password, or exchanged a refresh token.
    /// </summary>
    public class AuthenticationTokensResponseViewModel
    {
        /// <summary>
        /// Gets or sets the application's own JWT access token.
        /// </summary>
        public string AccessToken { get; set; } = null!;

        /// <summary>
        /// Gets or sets the opaque refresh token used to obtain a new access token once it expires.
        /// </summary>
        public string RefreshToken { get; set; } = null!;
    }
}
