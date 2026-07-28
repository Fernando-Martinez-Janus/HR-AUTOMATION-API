namespace HR_AUTOMATION.Infrastructure.Authentication
{
    /// <summary>
    /// Represents the identity information extracted from a validated Google ID token.
    /// </summary>
    public class GoogleUserInfo
    {
        /// <summary>
        /// Gets or sets the user's email address, as verified by Google.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user's full name, as provided by Google.
        /// </summary>
        public string Name { get; set; } = null!;
    }
}
