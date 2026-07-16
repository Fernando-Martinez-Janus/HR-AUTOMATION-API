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
    /// Provides business logic for managing skill categories.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="sharedRepository">The shared repository instance.</param>
    /// <param name="cacheService">The cache service instance.</param>
    /// <param name="configuration">The application configuration provider.</param>
    /// <param name="httpContextService">The HTTP context service instance.</param>
    /// <param name="notificationHub">The SignalR notification hub context.</param>
    public class SkillCategoryService(
        ILogger<SkillCategoryService> logger,
        ISharedRepository sharedRepository,
        ICacheService cacheService,
        IConfiguration configuration,
        IHttpContextService httpContextService,
        IHubContext<NotificationHub> notificationHub
    ) : ISkillCategoryService
    {
        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<SkillCategoryService> _logger = logger;

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
        /// Defines the default expiration time used for cached skill category data.
        /// </summary>
        private readonly TimeSpan _cacheDefaultExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisDefaultExpiration));

        /// <summary>
        /// Defines the long expiration time used for cached skill category data.
        /// </summary>
        private readonly TimeSpan _cacheLongExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisLongExpiration));

        /// <summary>
        /// Normalizes and validates the input model before processing.
        /// Throws a validation exception if the model is invalid.
        /// </summary>
        private void ValidateModel(SkillCategoryInputModel model)
        {
            int? organizationId = _httpContextService.GetOrganizationId();

            model.Normalize();
            model.OrganizationId ??= organizationId;

            ValidationResult validationResult = new SkillCategoryValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                ValidationFailure validationError = validationResult.Errors.First();
                Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);

                throw new ResponseExceptionFactory(currentException);
            }
        }

        /// <summary>
        /// Updates the cache version and notifies the appropriate SignalR groups that skill categories have changed.
        /// </summary>
        /// <param name="organizationId">The organization identifier to notify. If <see langword="null"/>, only the global notification is sent.</param>
        /// <param name="id">The item Id to delete (optional).</param>
        private async Task HandleChangedAsync(int? organizationId = null, int? id = null)
        {
            if (id.HasValue)
            {
                string idKey = SkillCategoryCacheKeys.ById(id.Value);

                await _cacheService.DeleteAsync(idKey);
            }

            List<string> notifyTo = [HubConstants.NotificationAllOrganizationsGroup];

            if (organizationId.HasValue)
            {
                notifyTo.Add(organizationId.Value.ToString());
                await _cacheService.SetAsync(SkillCategoryCacheKeys.Version(organizationId.Value), CacheKeyHelper.GenerateVersion());
            }

            await _cacheService.SetAsync(SkillCategoryCacheKeys.Version(), CacheKeyHelper.GenerateVersion());
            await _notificationHub.Clients.Groups(notifyTo).SendAsync(HubKeys.SkillCategoryChanged);
        }

        /// <summary>
        /// Retrieves skill categories matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching skill categories.</returns>
        public async Task<IEnumerable<SkillCategoryViewModel>> SearchAsync(SkillCategorySearchInputModel model)
        {
            try
            {
                int? organizationId = _httpContextService.GetOrganizationId();

                model.Normalize();
                model.OrganizationId ??= organizationId;

                string versionKey = SkillCategoryCacheKeys.Version(model.OrganizationId);
                string? version = await _cacheService.GetAsync<string>(versionKey);

                if (string.IsNullOrWhiteSpace(version))
                {
                    version = CacheKeyHelper.GenerateVersion();

                    await _cacheService.SetAsync(versionKey, version);
                }

                string searchKey = SkillCategoryCacheKeys.Search(model, version);
                IEnumerable<SkillCategoryViewModel>? cacheResult = await _cacheService.GetAsync<IEnumerable<SkillCategoryViewModel>>(searchKey);

                if (cacheResult != null)
                {
                    return cacheResult;
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_search_term", model.SearchTerm)
                ];

                IEnumerable<SkillCategoryModel> result = await _sharedRepository.QueryAsync<SkillCategoryModel>("[config].[web_get_skill_categories]", parameters);

                IEnumerable<SkillCategoryViewModel> mappedResult = Mapping.Mapper.Map<IEnumerable<SkillCategoryViewModel>>(result);

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
        /// Retrieves a skill category by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the skill category.</param>
        /// <returns>The requested <see cref="SkillCategoryViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified skill category does not exist.</exception>
        public async Task<SkillCategoryViewModel> GetAsync(int id)
        {
            try
            {
                string idKey = SkillCategoryCacheKeys.ById(id);

                SkillCategoryViewModel? cacheResult = await _cacheService.GetAsync<SkillCategoryViewModel>(idKey);

                if (cacheResult != null)
                {
                    return cacheResult;
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id)
                ];

                SkillCategoryModel result =
                    await _sharedRepository.QuerySingleAsync<SkillCategoryModel>("[config].[web_get_skill_category_by_id]", parameters)
                    ?? throw new ResponseExceptionFactory(Exceptions.SkillCategoryNotFound);

                SkillCategoryViewModel mappedResult = Mapping.Mapper.Map<SkillCategoryViewModel>(result);

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
        /// Creates a new skill category.
        /// </summary>
        /// <param name="model">The skill category information.</param>
        /// <returns>The identifier of the newly created skill category.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the skill category cannot be created.</exception>
        public async Task<int> CreateAsync(SkillCategoryInputModel model)
        {
            try
            {
                ValidateModel(model);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_name", model.CategoryName),
                    new("@p_icon_name", model.IconName),
                    new("@p_created_by", _httpContextService.GetUserId()),
                ];

                SkillCategoryModel result =
                   await _sharedRepository.QuerySingleAsync<SkillCategoryModel>("[config].[web_insert_skill_category]", parameters)
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
        /// Updates an existing skill category.
        /// </summary>
        /// <param name="id">The identifier of the skill category to update.</param>
        /// <param name="model">The updated skill category information.</param>
        public async Task UpdateAsync(int id, SkillCategoryInputModel model)
        {
            try
            {
                ValidateModel(model);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id),
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_name", model.CategoryName),
                    new("@p_icon_name", model.IconName),
                    new("@p_updated_by", _httpContextService.GetUserId()),
                ];

                await _sharedRepository.ExecuteAsync("[config].[web_update_skill_category]", parameters);

                await HandleChangedAsync(model.OrganizationId, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(UpdateAsync));
                throw;
            }
        }

        /// <summary>
        /// Deletes an existing skill category.
        /// </summary>
        /// <param name="id">The identifier of the skill category to delete.</param>
        public async Task DeleteAsync(int id)
        {
            try
            {
                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id),
                    new("@p_updated_by", _httpContextService.GetUserId()),
                ];

                SkillCategoryModel result =
                   await _sharedRepository.QuerySingleAsync<SkillCategoryModel>("[config].[web_delete_skill_category]", parameters)
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
        /// Updates the sort order of a skill category within an organization.
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

                await _sharedRepository.ExecuteAsync("[config].[web_reorder_skill_categories]", parameters);

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