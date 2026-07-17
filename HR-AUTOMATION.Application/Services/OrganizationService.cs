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
using Shared.Kernel.ViewModels;

namespace HR_AUTOMATION.Application.Services
{
    /// <summary>
    /// Provides business logic for managing organizations.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="sharedRepository">The shared repository instance.</param>
    /// <param name="cacheService">The cache service instance.</param>
    /// <param name="configuration">The application configuration provider.</param>
    /// <param name="httpContextService">The HTTP context service instance.</param>
    /// <param name="notificationHub">The SignalR notification hub context.</param>
    public class OrganizationService(
        ILogger<OrganizationService> logger,
        ISharedRepository sharedRepository,
        ICacheService cacheService,
        IConfiguration configuration,
        IHttpContextService httpContextService,
        IHubContext<NotificationHub> notificationHub
    ) : IOrganizationService
    {
        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<OrganizationService> _logger = logger;

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
        /// Defines the default expiration time used for cached organization data.
        /// </summary>
        private readonly TimeSpan _cacheDefaultExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisDefaultExpiration));

        /// <summary>
        /// Defines the long expiration time used for cached organization data.
        /// </summary>
        private readonly TimeSpan _cacheLongExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisLongExpiration));

        /// <summary>
        /// Normalizes and validates the input model before processing.
        /// Throws a validation exception if the model is invalid.
        /// </summary>
        private void ValidateModel(OrganizationInputModel model)
        {
            model.Normalize();

            ValidationResult validationResult = new OrganizationValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                ValidationFailure validationError = validationResult.Errors.First();
                Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);

                throw new ResponseExceptionFactory(currentException);
            }
        }

        /// <summary>
        /// Updates the cache version and notifies all SignalR groups that organizations have changed.
        /// </summary>
        /// <param name="id">The organization identifier to remove from cache (optional).</param>
        private async Task HandleChangedAsync(int? id = null)
        {
            if (id.HasValue)
            {
                string idKey = OrganizationCacheKeys.ById(id.Value);

                await _cacheService.DeleteAsync(idKey);
            }

            await _cacheService.SetAsync(OrganizationCacheKeys.Version(), CacheKeyHelper.GenerateVersion());
            await _notificationHub.Clients.Groups(HubConstants.NotificationAllOrganizationsGroup).SendAsync(HubKeys.OrganizationChanged);
        }

        /// <summary>
        /// Retrieves organizations matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A paginated collection of matching organizations.</returns>
        public async Task<PaginationResponse<OrganizationViewModel>> SearchAsync(OrganizationSearchInputModel model)
        {
            try
            {
                model.Normalize();

                string versionKey = OrganizationCacheKeys.Version();
                string? version = await _cacheService.GetAsync<string>(versionKey);

                if (string.IsNullOrWhiteSpace(version))
                {
                    version = CacheKeyHelper.GenerateVersion();

                    await _cacheService.SetAsync(versionKey, version);
                }

                string searchKey = OrganizationCacheKeys.Search(model, version);
                PaginationResponse<OrganizationViewModel>? cacheResult = await _cacheService.GetAsync<PaginationResponse<OrganizationViewModel>>(searchKey);

                if (cacheResult != null)
                {
                    return cacheResult;
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_page_number", model.PageNumber),
                    new("@p_page_size", model.PageSize),
                    new("@p_search_term", model.SearchTerm)
                ];

                IEnumerable<OrganizationModel> result = await _sharedRepository.QueryAsync<OrganizationModel>("[auth].[web_get_organizations]", parameters);

                IEnumerable<OrganizationViewModel> mappedResult = Mapping.Mapper.Map<IEnumerable<OrganizationViewModel>>(result);

                PaginationResponse<OrganizationViewModel> paginationResult = new(result.FirstOrDefault()?.TotalCount ?? 0, mappedResult);

                await _cacheService.SetAsync(searchKey, paginationResult, _cacheDefaultExpiration);

                return paginationResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(SearchAsync));
                throw;
            }
        }

        /// <summary>
        /// Retrieves an organization by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the organization.</param>
        /// <returns>The requested <see cref="OrganizationViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified organization does not exist.</exception>
        public async Task<OrganizationViewModel> GetAsync(int id)
        {
            try
            {
                string idKey = OrganizationCacheKeys.ById(id);

                OrganizationViewModel? cacheResult = await _cacheService.GetAsync<OrganizationViewModel>(idKey);

                if (cacheResult != null)
                {
                    return cacheResult;
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id)
                ];

                OrganizationModel result =
                    await _sharedRepository.QuerySingleAsync<OrganizationModel>("[auth].[web_get_organization_by_id]", parameters)
                    ?? throw new ResponseExceptionFactory(Exceptions.OrganizationNotFound);

                OrganizationViewModel mappedResult = Mapping.Mapper.Map<OrganizationViewModel>(result);

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
        /// Creates a new organization.
        /// </summary>
        /// <param name="model">The organization information.</param>
        /// <returns>The identifier of the newly created organization.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the organization cannot be created.</exception>
        public async Task<int> CreateAsync(OrganizationInputModel model)
        {
            try
            {
                ValidateModel(model);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_name", model.OrganizationName),
                    new("@p_slug", model.Slug),
                ];

                OrganizationModel result =
                   await _sharedRepository.QuerySingleAsync<OrganizationModel>("[auth].[web_insert_organization]", parameters)
                    ?? throw new ResponseExceptionFactory(Exceptions.InternalServerError);

                await HandleChangedAsync();

                return result.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CreateAsync));
                throw;
            }
        }

        /// <summary>
        /// Updates an existing organization.
        /// </summary>
        /// <param name="id">The identifier of the organization to update.</param>
        /// <param name="model">The updated organization information.</param>
        public async Task UpdateAsync(int id, OrganizationInputModel model)
        {
            try
            {
                ValidateModel(model);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id),
                    new("@p_name", model.OrganizationName),
                    new("@p_slug", model.Slug),
                ];

                await _sharedRepository.ExecuteAsync("[auth].[web_update_organization]", parameters);

                await HandleChangedAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(UpdateAsync));
                throw;
            }
        }

        /// <summary>
        /// Deletes an existing organization.
        /// </summary>
        /// <param name="id">The identifier of the organization to delete.</param>
        public async Task DeleteAsync(int id)
        {
            try
            {
                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id),
                ];

                OrganizationModel result =
                   await _sharedRepository.QuerySingleAsync<OrganizationModel>("[auth].[web_delete_organization]", parameters)
                    ?? throw new ResponseExceptionFactory(Exceptions.InternalServerError);

                await HandleChangedAsync(result.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(DeleteAsync));
                throw;
            }
        }
    }
}
