namespace HR_AUTOMATION.Application.InputModels
{
    /// <summary>
    /// Represents the request payload for authenticating a user with a Google ID token.
    /// </summary>
    public class GoogleLoginInputModel
    {
        /// <summary>
        /// Gets or sets the Google ID token issued by Google Identity Services.
        /// </summary>
        public string IdToken { get; set; } = null!;

        /// <summary>
        /// Normalizes the input model before processing.
        /// </summary>
        public void Normalize()
        {
            IdToken = IdToken?.Trim()!;
        }
    }
}
