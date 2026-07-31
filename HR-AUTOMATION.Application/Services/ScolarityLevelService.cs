using FluentValidation.Results;
using HR_AUTOMATION.Application.Cache;
using HR_AUTOMATION.Application.Hubs;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.Mapper;
using HR_AUTOMATION.Application.Validators;
using HR_AUTOMATION.Application.ViewModels;
using HR_AUTOMATION.Domain.Models;
using HR_AUTOMATION.Infrastructure.Constants;
using HR_AUTOMATION.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Kernel.IRepositories;
using Shared.Kernel.IServices;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Constants;
using Shared.Kernel.Utils.Enums;
using Shared.Kernel.Utils.Helpers;

namespace HR_AUTOMATION.Application.Services
{
    /// <summary>
    /// Provides business logic for managing scolarity levels.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="sharedRepository">The shared repository instance.</param>
    /// <param name="cacheService">The cache service instance.</param>
    /// <param name="configuration">The application configuration provider.</param>
    /// <param name="httpContextService">The HTTP context service instance.</param>
    /// <param name="notificationHub">The SignalR notification hub context.</param>
    public class ScolarityLevelService(
        ILogger<ScolarityLevelService> logger,
        ISharedRepository sharedRepository,
        ICacheService cacheService,
        IConfiguration configuration,
        IHttpContextService httpContextService,
        IHubContext<NotificationHub> notificationHub
    ) : IScolarityLevelService
    {
        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<ScolarityLevelService> _logger = logger;

        /// <summary>
        /// Provides access to shared data operations.
        /// </summary>
        private readonly ISharedRepository _sharedRepository = sharedRepository;

        /// <summary>
        /// Provides methods for interacting with the application cache.
        /// </summary>
        private readonly ICacheService _cacheService = cacheService;

        /// <summary>
        /// Provides access to HTTP context information, such as the current user and organization.
        /// </summary>
        private readonly IHttpContextService _httpContextService = httpContextService;

        /// <summary>
        /// Provides access to the SignalR notification hub for sending real-time updates.
        /// </summary>
        private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;

        /// <summary>
        /// Defines the default expiration time used for cached scolarity level data.
        /// </summary>
        private readonly TimeSpan _cacheDefaultExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisDefaultExpiration));

        /// <summary>
        /// Defines the long expiration time used for cached scolarity level data.
        /// </summary>
        private readonly TimeSpan _cacheLongExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisLongExpiration));

        /// <summary>
        /// Normalizes and validates the input model before processing.
        /// Throws a validation exception if the model is invalid.
        /// </summary>
        private void ValidateModel(ScolarityLevelInputModel model)
        {
            int? organizationId = _httpContextService.GetOrganizationId();

            model.Normalize();
            model.OrganizationId ??= organizationId;

            ValidationResult validationResult = new ScolarityLevelValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                ValidationFailure validationError = validationResult.Errors.First();
                Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);

                throw new ResponseExceptionFactory(currentException);
            }
        }

        /// <summary>
        /// Updates the cache version and notifies the appropriate SignalR groups that scolarity levels have changed.
        /// </summary>
        /// <param name="organizationId">The organization identifier to notify. If <see langword="null"/>, only the global notification is sent.</param>
        /// <param name="id">The item Id to delete (optional).</param>
        private async Task HandleChangedAsync(int? organizationId = null, int? id = null)
        {
            if (id.HasValue)
            {
                string idKey = ScolarityLevelCacheKeys.ById(id.Value);

                await _cacheService.DeleteAsync(idKey);
            }

            List<string> notifyTo = [HubConstants.NotificationAllOrganizationsGroup];

            if (organizationId.HasValue)
            {
                notifyTo.Add(organizationId.Value.ToString());
                await _cacheService.SetAsync(ScolarityLevelCacheKeys.Version(organizationId.Value), CacheKeyHelper.GenerateVersion());
            }

            await _cacheService.SetAsync(ScolarityLevelCacheKeys.Version(), CacheKeyHelper.GenerateVersion());
            await _notificationHub.Clients.Groups(notifyTo).SendAsync(HubKeys.ScolarityLevelChanged);
        }

        /// <summary>
        /// Retrieves scolarity levels matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching scolarity levels.</returns>
        public async Task<IEnumerable<ScolarityLevelViewModel>> SearchAsync(ScolarityLevelSearchInputModel model)
        {
            try
            {
                int? organizationId = _httpContextService.GetOrganizationId();

                model.Normalize();
                model.OrganizationId ??= organizationId;

                string versionKey = ScolarityLevelCacheKeys.Version(model.OrganizationId);
                string? version = await _cacheService.GetAsync<string>(versionKey);

                if (string.IsNullOrWhiteSpace(version))
                {
                    version = CacheKeyHelper.GenerateVersion();
                    await _cacheService.SetAsync(versionKey, version);
                }

                string searchKey = ScolarityLevelCacheKeys.Search(model, version);
                IEnumerable<ScolarityLevelViewModel>? cacheResult = await _cacheService.GetAsync<IEnumerable<ScolarityLevelViewModel>>(searchKey);

                if (cacheResult != null)
                {
                    return cacheResult;
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_search_term", model.SearchTerm)
                ];

                IEnumerable<ScolarityLevelModel> result = await _sharedRepository.QueryAsync<ScolarityLevelModel>("[config].[web_get_scolarity_levels]", parameters);

                IEnumerable<ScolarityLevelViewModel> mappedResult = Mapping.Mapper.Map<IEnumerable<ScolarityLevelViewModel>>(result);

                await _cacheService.SetAsync(searchKey, mappedResult, _cacheDefaultExpiration);

                return mappedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(SearchAsync));
                throw;
            }
        }

        /// <summary>
        /// Retrieves a scolarity level by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the scolarity level.</param>
        /// <returns>The requested <see cref="ScolarityLevelViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified scolarity level does not exist.</exception>
        public async Task<ScolarityLevelViewModel> GetAsync(int id)
        {
            try
            {
                string idKey = ScolarityLevelCacheKeys.ById(id);

                ScolarityLevelViewModel? cacheResult = await _cacheService.GetAsync<ScolarityLevelViewModel>(idKey);

                if (cacheResult != null)
                {
                    return cacheResult;
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id)
                ];

                ScolarityLevelModel result =
                    await _sharedRepository.QuerySingleAsync<ScolarityLevelModel>("[config].[web_get_scolarity_level_by_id]", parameters)
                    ?? throw new ResponseExceptionFactory(Exceptions.InternalServerError);

                ScolarityLevelViewModel mappedResult = Mapping.Mapper.Map<ScolarityLevelViewModel>(result);

                await _cacheService.SetAsync(idKey, mappedResult, _cacheLongExpiration);

                return mappedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetAsync));
                throw;
            }
        }

        /// <summary>
        /// Creates a new scolarity level.
        /// </summary>
        /// <param name="model">The scolarity level information.</param>
        /// <returns>The identifier of the newly created scolarity level.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the scolarity level cannot be created.</exception>
        public async Task<int> CreateAsync(ScolarityLevelInputModel model)
        {
            try
            {
                ValidateModel(model);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_name", model.LevelName),
                    new("@p_description", model.LevelDescription),
                    new("@p_created_by", _httpContextService.GetUserId()),
                ];

                ScolarityLevelModel result =
                    await _sharedRepository.QuerySingleAsync<ScolarityLevelModel>("[config].[web_insert_scolarity_level]", parameters)
                    ?? throw new ResponseExceptionFactory(Exceptions.InternalServerError);

                await HandleChangedAsync(model.OrganizationId);

                return result.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CreateAsync));
                throw;
            }
        }

        /// <summary>
        /// Updates an existing scolarity level.
        /// </summary>
        /// <param name="id">The identifier of the scolarity level to update.</param>
        /// <param name="model">The updated scolarity level information.</param>
        public async Task UpdateAsync(int id, ScolarityLevelInputModel model)
        {
            try
            {
                ValidateModel(model);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id),
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_name", model.LevelName),
                    new("@p_description", model.LevelDescription),
                    new("@p_updated_by", _httpContextService.GetUserId()),
                ];

                await _sharedRepository.ExecuteAsync("[config].[web_update_scolarity_level]", parameters);

                await HandleChangedAsync(model.OrganizationId, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(UpdateAsync));
                throw;
            }
        }

        /// <summary>
        /// Deletes an existing scolarity level.
        /// </summary>
        /// <param name="id">The identifier of the scolarity level to delete.</param>
        public async Task DeleteAsync(int id)
        {
            try
            {
                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id),
                    new("@p_updated_by", _httpContextService.GetUserId()),
                ];

                ScolarityLevelModel result =
                    await _sharedRepository.QuerySingleAsync<ScolarityLevelModel>("[config].[web_delete_scolarity_level]", parameters)
                    ?? throw new ResponseExceptionFactory(Exceptions.InternalServerError);

                await HandleChangedAsync(result.OrganizationId, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(DeleteAsync));
                throw;
            }
        }

        /// <summary>
        /// Updates the sort order of a scolarity level within an organization.
        /// </summary>
        /// <param name="model">The reorder request information.</param>
        /// <exception cref="ResponseExceptionFactory">Thrown when the required organization information is missing or the operation fails.</exception>
        public async Task ReorderAsync(ReorderInputModel model)
        {
            try
            {
                model.OrganizationId ??= _httpContextService.GetOrganizationId();

                if (!model.OrganizationId.HasValue)
                {
                    throw new ResponseExceptionFactory(Exceptions.OrganizationRequired);
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", model.ItemId),
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_new_sort_order", model.NewSortOrder),
                    new("@p_updated_by", _httpContextService.GetUserId()),
                ];

                await _sharedRepository.ExecuteAsync("[config].[web_reorder_scolarity_levels]", parameters);

                await HandleChangedAsync(model.OrganizationId, model.ItemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(ReorderAsync));
                throw;
            }
        }
    }
}
