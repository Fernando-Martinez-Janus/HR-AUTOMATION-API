using HR_AUTOMATION.Domain.Entities;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Generates and persists opaque refresh tokens, independent of JWT access token generation.
    /// </summary>
    public interface IRefreshTokenService
    {
        /// <summary>
        /// Generates a cryptographically secure, opaque refresh token and persists it for the
        /// specified user.
        /// </summary>
        /// <param name="userId">The identifier of the user the token belongs to.</param>
        /// <param name="ipAddress">The IP address the token was issued from, if available.</param>
        /// <param name="userAgent">The user agent the token was issued from, if available.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The generated refresh token.</returns>
        Task<string> GenerateAsync(int userId, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a refresh token and validates that it is enabled, not revoked, and not expired.
        /// Reserved for the future token-refresh endpoint.
        /// </summary>
        /// <param name="token">The refresh token to validate.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The refresh token record if it is valid; otherwise, <c>null</c>.</returns>
        Task<RefreshToken?> ValidateAsync(string token, CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes a refresh token, optionally recording the token that replaced it (rotation).
        /// Reserved for the future token-refresh endpoint.
        /// </summary>
        /// <param name="token">The refresh token to revoke.</param>
        /// <param name="replacedByToken">The token that replaces this one, if issued as part of a rotation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        Task RevokeAsync(string token, string? replacedByToken, CancellationToken cancellationToken = default);
    }
}
