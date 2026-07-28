using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Domain.Entities;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Kernel.IRepositories;
using Shared.Kernel.Utils.Constants;
using System.Security.Cryptography;

namespace HR_AUTOMATION.Application.Services
{
    /// <summary>
    /// Generates and persists opaque refresh tokens. Unlike <see cref="IJwtTokenService"/>, this
    /// service never encodes claims into the token itself; the token is a random value whose only
    /// meaning comes from the database row it is looked up against.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="configuration">The application configuration provider.</param>
    /// <param name="sharedRepository">The shared repository instance.</param>
    public class RefreshTokenService(
        ILogger<RefreshTokenService> logger,
        IConfiguration configuration,
        ISharedRepository sharedRepository
    ) : IRefreshTokenService
    {
        /// <summary>
        /// Number of cryptographically random bytes used to build each refresh token.
        /// </summary>
        private const int TokenByteSize = 64;

        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<RefreshTokenService> _logger = logger;

        /// <summary>
        /// The refresh token lifetime, in days.
        /// </summary>
        private readonly int _expirationDays = configuration.GetValue<int>(AppConstants.RefreshTokenExpirationDaysKey);

        /// <summary>
        /// Provides access to shared data operations.
        /// </summary>
        private readonly ISharedRepository _sharedRepository = sharedRepository;

        /// <summary>
        /// Generates a cryptographically secure, opaque refresh token and persists it for the
        /// specified user.
        /// </summary>
        /// <param name="userId">The identifier of the user the token belongs to.</param>
        /// <param name="ipAddress">The IP address the token was issued from, if available.</param>
        /// <param name="userAgent">The user agent the token was issued from, if available.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The generated refresh token.</returns>
        public async Task<string> GenerateAsync(int userId, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default)
        {
            try
            {
                byte[] randomBytes = RandomNumberGenerator.GetBytes(TokenByteSize);
                string token = WebEncoders.Base64UrlEncode(randomBytes);

                DateTime expiresAt = DateTime.UtcNow.AddDays(_expirationDays);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_user_id", userId),
                    new("@p_token", token),
                    new("@p_expires_at", expiresAt),
                    new("@p_ip_address", ipAddress),
                    new("@p_user_agent", userAgent),
                    new("@p_created_by", userId)
                ];

                await _sharedRepository.ExecuteAsync("[auth].[web_create_refresh_token]", parameters, cancellationToken);

                return token;
            }
            catch (Exception ex)
            {
                // Never include the token value itself in logs.
                _logger.LogError(ex, nameof(GenerateAsync));
                throw;
            }
        }

        /// <summary>
        /// Retrieves a refresh token and validates that it is enabled, not revoked, and not expired.
        /// Reserved for the future token-refresh endpoint.
        /// </summary>
        /// <param name="token">The refresh token to validate.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The refresh token record if it is valid; otherwise, <c>null</c>.</returns>
        public async Task<RefreshToken?> ValidateAsync(string token, CancellationToken cancellationToken = default)
        {
            try
            {
                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_token", token)
                ];

                RefreshToken? refreshToken = await _sharedRepository.QuerySingleAsync<RefreshToken>("[auth].[web_get_refresh_token]", parameters, cancellationToken);

                bool isValid = refreshToken is { IsEnabled: true, IsRevoked: false } && refreshToken.ExpiresAt > DateTime.UtcNow;

                return isValid ? refreshToken : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(ValidateAsync));
                throw;
            }
        }

        /// <summary>
        /// Revokes a refresh token, optionally recording the token that replaced it (rotation).
        /// Reserved for the future token-refresh endpoint.
        /// </summary>
        /// <param name="token">The refresh token to revoke.</param>
        /// <param name="replacedByToken">The token that replaces this one, if issued as part of a rotation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        public async Task RevokeAsync(string token, string? replacedByToken, CancellationToken cancellationToken = default)
        {
            try
            {
                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_token", token),
                    new("@p_replaced_by_token", replacedByToken)
                ];

                await _sharedRepository.ExecuteAsync("[auth].[web_revoke_refresh_token]", parameters, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(RevokeAsync));
                throw;
            }
        }
    }
}
