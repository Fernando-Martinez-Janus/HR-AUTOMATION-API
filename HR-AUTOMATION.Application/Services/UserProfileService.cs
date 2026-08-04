using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.ViewModels;
using HR_AUTOMATION.Domain.Models;
using Microsoft.Extensions.Logging;
using Shared.Kernel.IRepositories;
using Shared.Kernel.IServices;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Services
{
    /// <summary>
    /// Retrieves user profile information (role, organization, permissions). Responsible ONLY
    /// for profile retrieval; authentication (Google/email login, JWT, refresh tokens) lives in
    /// <see cref="IAuthenticationService"/>.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="httpContextService">Provides the authenticated user's identifier from JWT claims.</param>
    /// <param name="sharedRepository">The shared repository instance.</param>
    public class UserProfileService(
        ILogger<UserProfileService> logger,
        IHttpContextService httpContextService,
        ISharedRepository sharedRepository
    ) : IUserProfileService
    {
        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<UserProfileService> _logger = logger;

        /// <summary>
        /// Provides the authenticated user's identifier from JWT claims.
        /// </summary>
        private readonly IHttpContextService _httpContextService = httpContextService;

        /// <summary>
        /// Provides access to shared data operations.
        /// </summary>
        private readonly ISharedRepository _sharedRepository = sharedRepository;

        /// <summary>
        /// Retrieves the profile of the currently authenticated user, identified exclusively
        /// from the JWT claims of the current request.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The authenticated user's profile.</returns>
        /// <exception cref="ResponseExceptionFactory">
        /// Thrown when the current request has no authenticated user identifier, or the user no longer exists.
        /// </exception>
        public async Task<UserProfileViewModel> GetMyProfileAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                int userId = _httpContextService.GetAuthenticatedUserId()
                    ?? throw new ResponseExceptionFactory(Exceptions.UserNotFound);

                return await LoadProfileAsync(userId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetMyProfileAsync));
                throw;
            }
        }

        /// <summary>
        /// Retrieves the profile of any user by identifier. Intended for administrator-only access.
        /// </summary>
        /// <param name="userId">The identifier of the user to load.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The requested user's profile.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the user does not exist.</exception>
        public async Task<UserProfileViewModel> GetByIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await LoadProfileAsync(userId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetByIdAsync));
                throw;
            }
        }

        /// <summary>
        /// Loads a user's role, organization, and permissions by identifier, reusing the same
        /// stored procedure the refresh token flow uses to re-hydrate a user by id.
        /// </summary>
        private async Task<UserProfileViewModel> LoadProfileAsync(int userId, CancellationToken cancellationToken)
        {
            List<KeyValuePair<string, object?>> parameters = [
                new("@p_user_id", userId)
            ];

            IEnumerable<UserPermissionRow> rows = await _sharedRepository.QueryAsync<UserPermissionRow>("[auth].[web_get_user_by_id]", parameters, cancellationToken);

            UserPermissionRow? firstRow = rows.FirstOrDefault();

            if (firstRow is null)
            {
                throw new ResponseExceptionFactory(Exceptions.UserNotFound);
            }

            return new UserProfileViewModel
            {
                Id = firstRow.Id,
                Name = firstRow.FullName,
                Email = firstRow.Email,
                RoleId = firstRow.RoleId,
                RoleName = firstRow.RoleName,
                OrganizationId = firstRow.OrganizationId,
                OrganizationName = firstRow.OrganizationName,
                Permissions = BuildPermissions(rows)
            };
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
    }
}
