using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.ViewModels;
using HR_AUTOMATION.Domain.Entities;
using HR_AUTOMATION.Domain.Models;
using HR_AUTOMATION.Infrastructure.Authentication;
using Microsoft.Extensions.Logging;
using Shared.Kernel.IRepositories;
using Shared.Kernel.IServices;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Services
{
    /// <summary>
    /// Authenticates users and issues the application's own JWT plus a persisted refresh token,
    /// regardless of the identity source (Google Sign-In or email/password). Both login flows
    /// validate identity in their own way and then share the exact same JWT generation and
    /// refresh token issuance logic. This service is responsible ONLY for authentication; user
    /// profile retrieval (role, organization, permissions) lives in <see cref="IUserProfileService"/>.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="googleTokenValidator">Validates the incoming Google ID token.</param>
    /// <param name="jwtTokenService">Generates the application's own JWT access tokens.</param>
    /// <param name="passwordHasherService">Verifies email/password credentials against the stored hash.</param>
    /// <param name="refreshTokenService">Generates and persists opaque refresh tokens.</param>
    /// <param name="httpContextService">Provides access to the current request's IP address and user agent.</param>
    /// <param name="sharedRepository">The shared repository instance.</param>
    public class AuthenticationService(
        ILogger<AuthenticationService> logger,
        IGoogleTokenValidator googleTokenValidator,
        IJwtTokenService jwtTokenService,
        IPasswordHasherService passwordHasherService,
        IRefreshTokenService refreshTokenService,
        IHttpContextService httpContextService,
        ISharedRepository sharedRepository
    ) : IAuthenticationService
    {
        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<AuthenticationService> _logger = logger;

        /// <summary>
        /// Validates the incoming Google ID token.
        /// </summary>
        private readonly IGoogleTokenValidator _googleTokenValidator = googleTokenValidator;

        /// <summary>
        /// Generates the application's own JWT access tokens.
        /// </summary>
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

        /// <summary>
        /// Verifies email/password credentials against the stored hash.
        /// </summary>
        private readonly IPasswordHasherService _passwordHasherService = passwordHasherService;

        /// <summary>
        /// Generates and persists opaque refresh tokens.
        /// </summary>
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;

        /// <summary>
        /// Provides access to the current request's IP address and user agent.
        /// </summary>
        private readonly IHttpContextService _httpContextService = httpContextService;

        /// <summary>
        /// Provides access to shared data operations.
        /// </summary>
        private readonly ISharedRepository _sharedRepository = sharedRepository;

        /// <summary>
        /// Validates the Google ID token, authenticates the user against the application's database,
        /// and issues an application JWT access token.
        /// </summary>
        /// <param name="model">The Google Sign-In request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The application access token and refresh token.</returns>
        /// <exception cref="ResponseExceptionFactory">
        /// Thrown when the token is missing or invalid, or the user does not exist or is inactive.
        /// </exception>
        public async Task<AuthenticationTokensResponseViewModel> LoginWithGoogleAsync(GoogleLoginInputModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                model.Normalize();

                GoogleUserInfo googleUser = await _googleTokenValidator.ValidateAsync(model.IdToken, cancellationToken);

                if (string.IsNullOrWhiteSpace(googleUser?.Email))
                {
                    _logger.LogWarning("LoginWithGoogleAsync: empty or null email in Google token.");
                    throw new ResponseExceptionFactory(Exceptions.InvalidGoogleToken);
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_email", googleUser.Email)
                ];

                IEnumerable<UserPermissionRow> rows = await _sharedRepository.QueryAsync<UserPermissionRow>("[auth].[web_get_user_by_email]", parameters, cancellationToken);

                UserPermissionRow? firstRow = rows.FirstOrDefault();

                if (firstRow is null)
                {
                    _logger.LogInformation("LoginWithGoogleAsync: user not found for email {Email}", googleUser.Email);
                    throw new ResponseExceptionFactory(Exceptions.UserNotFound);
                }

                if (!firstRow.IsActive)
                {
                    _logger.LogInformation("LoginWithGoogleAsync: user {Email} is inactive", googleUser.Email);
                    throw new ResponseExceptionFactory(Exceptions.UserInactive);
                }

                return await BuildAuthenticatedResponseAsync(firstRow, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(LoginWithGoogleAsync));
                throw;
            }
        }

        /// <summary>
        /// Validates the email and password against the application's database
        /// and issues an application JWT access token.
        /// </summary>
        /// <param name="model">The email/password login request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The application access token and refresh token.</returns>
        /// <exception cref="ResponseExceptionFactory">
        /// Thrown when the email/password is missing or invalid, or the user is inactive.
        /// </exception>
        public async Task<AuthenticationTokensResponseViewModel> LoginWithEmailAsync(LoginInputModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                model.Normalize();

                if (string.IsNullOrWhiteSpace(model.Email))
                {
                    throw new ResponseExceptionFactory(Exceptions.EmailRequired);
                }

                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    throw new ResponseExceptionFactory(Exceptions.PasswordRequired);
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_email", model.Email)
                ];

                IEnumerable<UserPermissionRow> rows = await _sharedRepository.QueryAsync<UserPermissionRow>("[auth].[web_login]", parameters, cancellationToken);

                UserPermissionRow? firstRow = rows.FirstOrDefault();

                // A generic "invalid credentials" error is used for both a missing user and a
                // wrong password, so the response never reveals whether the email is registered.
                if (firstRow is null || string.IsNullOrWhiteSpace(firstRow.PasswordHash) ||
                    !_passwordHasherService.VerifyPassword(firstRow.PasswordHash, model.Password))
                {
                    _logger.LogInformation("LoginWithEmailAsync: invalid credentials for email {Email}", model.Email);
                    throw new ResponseExceptionFactory(Exceptions.InvalidCredentials);
                }

                if (!firstRow.IsActive)
                {
                    _logger.LogInformation("LoginWithEmailAsync: user {Email} is inactive", model.Email);
                    throw new ResponseExceptionFactory(Exceptions.UserInactive);
                }

                return await BuildAuthenticatedResponseAsync(firstRow, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(LoginWithEmailAsync));
                throw;
            }
        }

        /// <summary>
        /// Validates a refresh token, rotates it, and issues a new application JWT access token
        /// for the user it belongs to.
        /// </summary>
        /// <param name="model">The refresh token request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The application access token and refresh token.</returns>
        /// <exception cref="ResponseExceptionFactory">
        /// Thrown when the refresh token is missing, unknown, revoked, expired, or the user is inactive.
        /// </exception>
        public async Task<AuthenticationTokensResponseViewModel> RefreshAsync(RefreshTokenInputModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                model.Normalize();

                if (string.IsNullOrWhiteSpace(model.RefreshToken))
                {
                    throw new ResponseExceptionFactory(Exceptions.RefreshTokenRequired);
                }

                // ValidateAsync already collapses "not found", "revoked", and "expired" into a
                // single null result, so the response never reveals which of the three applied.
                RefreshToken? existingToken = await _refreshTokenService.ValidateAsync(model.RefreshToken, cancellationToken);

                if (existingToken is null)
                {
                    _logger.LogInformation("RefreshAsync: invalid, expired, or revoked refresh token.");
                    throw new ResponseExceptionFactory(Exceptions.InvalidCredentials);
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_user_id", existingToken.UserId)
                ];

                IEnumerable<UserPermissionRow> rows = await _sharedRepository.QueryAsync<UserPermissionRow>("[auth].[web_get_user_by_id]", parameters, cancellationToken);

                UserPermissionRow? firstRow = rows.FirstOrDefault();

                if (firstRow is null)
                {
                    _logger.LogInformation("RefreshAsync: user {UserId} not found.", existingToken.UserId);
                    throw new ResponseExceptionFactory(Exceptions.InvalidCredentials);
                }

                if (!firstRow.IsActive)
                {
                    _logger.LogInformation("RefreshAsync: user {UserId} is inactive.", existingToken.UserId);
                    throw new ResponseExceptionFactory(Exceptions.InvalidCredentials);
                }

                AuthenticationTokensResponseViewModel response = await BuildAuthenticatedResponseAsync(firstRow, cancellationToken);

                // Rotation: the old token is revoked and points at the new one via replaced_by_token.
                await _refreshTokenService.RevokeAsync(model.RefreshToken, response.RefreshToken, cancellationToken);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(RefreshAsync));
                throw;
            }
        }

        /// <summary>
        /// Revokes a refresh token. Always succeeds, even if the token does not exist or was
        /// already revoked, so logout remains idempotent from the client's perspective.
        /// </summary>
        /// <param name="model">The refresh token to revoke.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        public async Task LogoutAsync(RefreshTokenInputModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                model.Normalize();

                if (string.IsNullOrWhiteSpace(model.RefreshToken))
                {
                    return;
                }

                await _refreshTokenService.RevokeAsync(model.RefreshToken, replacedByToken: null, cancellationToken);
            }
            catch (Exception ex)
            {
                // Logout must always succeed from the caller's perspective; log and swallow.
                _logger.LogError(ex, nameof(LogoutAsync));
            }
        }

        /// <summary>
        /// Generates the JWT and refresh token, persists the refresh token, and builds the
        /// tokens-only response shared by every login/refresh method.
        /// </summary>
        /// <param name="user">The authenticated user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        private async Task<AuthenticationTokensResponseViewModel> BuildAuthenticatedResponseAsync(User user, CancellationToken cancellationToken)
        {
            (string accessToken, int _) = _jwtTokenService.GenerateToken(user);

            string? ipAddress = _httpContextService.GetIpAddress();
            string? userAgent = _httpContextService.GetUserAgent();

            string refreshToken = await _refreshTokenService.GenerateAsync(user.Id, ipAddress, userAgent, cancellationToken);

            return new AuthenticationTokensResponseViewModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
