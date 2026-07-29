namespace HR_AUTOMATION.Infrastructure.Authentication
{
    /// <summary>
    /// Validates Google ID tokens and extracts the authenticated identity.
    /// </summary>
    public interface IGoogleTokenValidator
    {
        /// <summary>
        /// Validates a Google ID token against Google's public keys and issuer/audience/expiration rules.
        /// </summary>
        /// <param name="idToken">The Google ID token to validate.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The identity information extracted from the validated token.</returns>
        Task<GoogleUserInfo> ValidateAsync(string idToken, CancellationToken cancellationToken = default);
    }
}
