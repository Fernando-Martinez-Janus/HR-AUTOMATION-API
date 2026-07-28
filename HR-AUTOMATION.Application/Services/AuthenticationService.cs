using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.ViewModels;
using HR_AUTOMATION.Domain.Entities;
using HR_AUTOMATION.Domain.Models;
using HR_AUTOMATION.Infrastructure.Authentication;
using Microsoft.Extensions.Logging;
using Shared.Kernel.IRepositories;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Constants;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Services
{
    /// <summary>
    /// Authenticates users and issues the application's own JWT, regardless of the
    /// identity source (Google Sign-In or email/password). Both login flows validate
    /// identity in their own way and then share the exact same JWT generation and
    /// response-building logic.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="googleTokenValidator">Validates the incoming Google ID token.</param>
    /// <param name="jwtTokenService">Generates the application's own JWT access tokens.</param>
    /// <param name="passwordHasherService">Verifies email/password credentials against the stored hash.</param>
    /// <param name="sharedRepository">The shared repository instance.</param>
    public class AuthenticationService(
        ILogger<AuthenticationService> logger,
        IGoogleTokenValidator googleTokenValidator,
        IJwtTokenService jwtTokenService,
        IPasswordHasherService passwordHasherService,
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
        /// Provides access to shared data operations.
        /// </summary>
        private readonly ISharedRepository _sharedRepository = sharedRepository;

        /// <summary>
        /// Validates the Google ID token, authenticates the user against the application's database,
        /// and issues an application JWT access token.
        /// </summary>
        /// <param name="model">The Google Sign-In request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The application access token and authenticated user information.</returns>
        /// <exception cref="ResponseExceptionFactory">
        /// Thrown when the token is missing or invalid, or the user does not exist or is inactive.
        /// </exception>
        public async Task<AuthenticationResponseViewModel> LoginWithGoogleAsync(GoogleLoginInputModel model, CancellationToken cancellationToken = default)
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

                Dictionary<string, bool> permissions = BuildPermissions(rows);

                return BuildAuthenticatedResponse(firstRow, permissions);
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
        /// <returns>The application access token and authenticated user information.</returns>
        /// <exception cref="ResponseExceptionFactory">
        /// Thrown when the email/password is missing or invalid, or the user is inactive.
        /// </exception>
        public async Task<AuthenticationResponseViewModel> LoginWithEmailAsync(LoginInputModel model, CancellationToken cancellationToken = default)
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

                Dictionary<string, bool> permissions = BuildPermissions(rows);

                return BuildAuthenticatedResponse(firstRow, permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(LoginWithEmailAsync));
                throw;
            }
        }

        /// <summary>
        /// Folds the stored procedure's one-row-per-permission result into a single
        /// dictionary, keyed by normalized permission name.
        /// </summary>
        private static Dictionary<string, bool> BuildPermissions(IEnumerable<UserPermissionRow> rows)
        {
            Dictionary<string, bool> permissions = [];

            foreach (UserPermissionRow row in rows)
            {
                if (!string.IsNullOrWhiteSpace(row.PermissionName) && row.IsAllowed.HasValue)
                {
                    string permissionKey = row.PermissionName.Trim().ToLowerInvariant();

                    permissions[permissionKey] = row.IsAllowed.Value;
                }
            }

            return permissions;
        }

        /// <summary>
        /// Generates the JWT and builds the authentication response shared by every login method.
        /// </summary>
        /// <param name="user">The authenticated user.</param>
        /// <param name="permissions">The user's permissions, keyed by permission name.</param>
        private AuthenticationResponseViewModel BuildAuthenticatedResponse(User user, Dictionary<string, bool> permissions)
        {
            (string accessToken, int expiresIn) = _jwtTokenService.GenerateToken(user);

            return new AuthenticationResponseViewModel
            {
                AccessToken = accessToken,
                ExpiresIn = expiresIn,
                TokenType = AppConstants.Bearer,
                User = new AuthenticatedUserViewModel
                {
                    Id = user.Id,
                    Name = user.FullName,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    Role = user.RoleName,
                    OrganizationId = user.OrganizationId,
                    OrganizationName = user.OrganizationName,
                    Permissions = permissions
                }
            };
        }
    }
}
