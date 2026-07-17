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
    /// Provides business logic for managing currencies.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="sharedRepository">The shared repository instance.</param>
    /// <param name="cacheService">The cache service instance.</param>
    /// <param name="configuration">The application configuration provider.</param>
    /// <param name="httpContextService">The HTTP context service instance.</param>
    /// <param name="notificationHub">The SignalR notification hub context.</param>
    public class CurrencyService(
        ILogger<CurrencyService> logger,
        ISharedRepository sharedRepository,
        ICacheService cacheService,
        IConfiguration configuration,
        IHttpContextService httpContextService,
        IHubContext<NotificationHub> notificationHub
    ) : ICurrencyService
    {
        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<CurrencyService> _logger = logger;

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
        /// Defines the default expiration time used for cached currency data.
        /// </summary>
        private readonly TimeSpan _cacheDefaultExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisDefaultExpiration));

        /// <summary>
        /// Defines the long expiration time used for cached currency data.
        /// </summary>
        private readonly TimeSpan _cacheLongExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisLongExpiration));

        /// <summary>
        /// Normalizes and validates the input model before processing.
        /// Throws a validation exception if the model is invalid.
        /// </summary>
        private void ValidateModel(CurrencyInputModel model)
        {
            int? organizationId = _httpContextService.GetOrganizationId();

            model.Normalize();
            model.OrganizationId ??= organizationId;

            ValidationResult validationResult = new CurrencyValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                ValidationFailure validationError = validationResult.Errors.First();
                Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);

                throw new ResponseExceptionFactory(currentException);
            }
        }

        /// <summary>
        /// Updates the cache version and notifies the appropriate SignalR groups that currencies have changed.
        /// </summary>
        /// <param name="organizationId">The organization identifier to notify. If <see langword="null"/>, only the global notification is sent.</param>
        /// <param name="id">The item Id to delete (optional).</param>
        private async Task HandleChangedAsync(int? organizationId = null, int? id = null)
        {
            if (id.HasValue)
            {
                string idKey = CurrencyCacheKeys.ById(id.Value);

                await _cacheService.DeleteAsync(idKey);
            }

            List<string> notifyTo = [HubConstants.NotificationAllOrganizationsGroup];

            if (organizationId.HasValue)
            {
                notifyTo.Add(organizationId.Value.ToString());
                await _cacheService.SetAsync(CurrencyCacheKeys.Version(organizationId.Value), CacheKeyHelper.GenerateVersion());
            }

            await _cacheService.SetAsync(CurrencyCacheKeys.Version(), CacheKeyHelper.GenerateVersion());
            await _notificationHub.Clients.Groups(notifyTo).SendAsync(HubKeys.CurrencyChanged);
        }

        /// <summary>
        /// Retrieves currencies matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching currencies.</returns>
        public async Task<IEnumerable<CurrencyViewModel>> SearchAsync(CurrencySearchInputModel model)
        {
            try
            {
                int? organizationId = _httpContextService.GetOrganizationId();

                model.Normalize();
                model.OrganizationId ??= organizationId;

                string versionKey = CurrencyCacheKeys.Version(model.OrganizationId);
                string? version = await _cacheService.GetAsync<string>(versionKey);

                if (string.IsNullOrWhiteSpace(version))
                {
                    version = CacheKeyHelper.GenerateVersion();

                    await _cacheService.SetAsync(versionKey, version);
                }

                string searchKey = CurrencyCacheKeys.Search(model, version);
                IEnumerable<CurrencyViewModel>? cacheResult = await _cacheService.GetAsync<IEnumerable<CurrencyViewModel>>(searchKey);

                if (cacheResult != null)
                {
                    return cacheResult;
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_search_term", model.SearchTerm)
                ];

                IEnumerable<CurrencyModel> result = await _sharedRepository.QueryAsync<CurrencyModel>("[config].[web_get_currencies]", parameters);

                IEnumerable<CurrencyViewModel> mappedResult = Mapping.Mapper.Map<IEnumerable<CurrencyViewModel>>(result);

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
        /// Retrieves a currency by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the currency.</param>
        /// <returns>The requested <see cref="CurrencyViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified currency does not exist.</exception>
        public async Task<CurrencyViewModel> GetAsync(int id)
        {
            try
            {
                string idKey = CurrencyCacheKeys.ById(id);

                CurrencyViewModel? cacheResult = await _cacheService.GetAsync<CurrencyViewModel>(idKey);

                if (cacheResult != null)
                {
                    return cacheResult;
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id)
                ];

                CurrencyModel result =
                    await _sharedRepository.QuerySingleAsync<CurrencyModel>("[config].[web_get_currency_by_id]", parameters)
                    ?? throw new ResponseExceptionFactory(Exceptions.CurrencyNotFound);

                CurrencyViewModel mappedResult = Mapping.Mapper.Map<CurrencyViewModel>(result);

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
        /// Creates a new currency.
        /// </summary>
        /// <param name="model">The currency information.</param>
        /// <returns>The identifier of the newly created currency.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the currency cannot be created.</exception>
        public async Task<int> CreateAsync(CurrencyInputModel model)
        {
            try
            {
                ValidateModel(model);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_code", model.CurrencyCode),
                    new("@p_name", model.CurrencyName),
                    new("@p_symbol", model.CurrencySymbol),
                    new("@p_created_by", _httpContextService.GetUserId()),
                ];

                CurrencyModel result =
                   await _sharedRepository.QuerySingleAsync<CurrencyModel>("[config].[web_insert_currency]", parameters)
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
        /// Updates an existing currency.
        /// </summary>
        /// <param name="id">The identifier of the currency to update.</param>
        /// <param name="model">The updated currency information.</param>
        public async Task UpdateAsync(int id, CurrencyInputModel model)
        {
            try
            {
                ValidateModel(model);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id),
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_code", model.CurrencyCode),
                    new("@p_name", model.CurrencyName),
                    new("@p_symbol", model.CurrencySymbol),
                    new("@p_updated_by", _httpContextService.GetUserId()),
                ];

                await _sharedRepository.ExecuteAsync("[config].[web_update_currency]", parameters);

                await HandleChangedAsync(model.OrganizationId, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(UpdateAsync));
                throw;
            }
        }

        /// <summary>
        /// Deletes an existing currency.
        /// </summary>
        /// <param name="id">The identifier of the currency to delete.</param>
        public async Task DeleteAsync(int id)
        {
            try
            {
                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id),
                    new("@p_updated_by", _httpContextService.GetUserId()),
                ];

                CurrencyModel result =
                   await _sharedRepository.QuerySingleAsync<CurrencyModel>("[config].[web_delete_currency]", parameters)
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
        /// Updates the sort order of a currency within an organization.
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

                await _sharedRepository.ExecuteAsync("[config].[web_reorder_currencies]", parameters);

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
