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
    /// Provides business logic for managing question categories.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="sharedRepository">The shared repository instance.</param>
    /// <param name="cacheService">The cache service instance.</param>
    /// <param name="configuration">The application configuration provider.</param>
    /// <param name="httpContextService">The HTTP context service instance.</param>
    /// <param name="notificationHub">The SignalR notification hub context.</param>
    public class QuestionCategoryService(
        ILogger<QuestionCategoryService> logger,
        ISharedRepository sharedRepository,
        ICacheService cacheService,
        IConfiguration configuration,
        IHttpContextService httpContextService,
        IHubContext<NotificationHub> notificationHub
    ) : IQuestionCategoryService
    {
        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<QuestionCategoryService> _logger = logger;

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
        /// Defines the default expiration time used for cached question category data.
        /// </summary>
        private readonly TimeSpan _cacheDefaultExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisDefaultExpiration));

        /// <summary>
        /// Defines the long expiration time used for cached question category data.
        /// </summary>
        private readonly TimeSpan _cacheLongExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisLongExpiration));

        /// <summary>
        /// Normalizes and validates the input model before processing.
        /// Throws a validation exception if the model is invalid.
        /// </summary>
        private void ValidateModel(QuestionCategoryInputModel model)
        {
            int? organizationId = _httpContextService.GetOrganizationId();

            model.Normalize();
            model.OrganizationId ??= organizationId;

            ValidationResult validationResult = new QuestionCategoryValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                ValidationFailure validationError = validationResult.Errors.First();
                Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);

                throw new ResponseExceptionFactory(currentException);
            }
        }

        /// <summary>
        /// Updates the cache version and notifies the appropriate SignalR groups that question categories have changed.
        /// </summary>
        /// <param name="organizationId">The organization identifier to notify. If <see langword="null"/>, only the global notification is sent.</param>
        /// <param name="id">The item Id to delete (optional).</param>
        private async Task HandleChangedAsync(int? organizationId = null, int? id = null)
        {
            if (id.HasValue)
            {
                string idKey = QuestionCategoryCacheKeys.ById(id.Value);

                await _cacheService.DeleteAsync(idKey);
            }

            List<string> notifyTo = [HubConstants.NotificationAllOrganizationsGroup];

            if (organizationId.HasValue)
            {
                notifyTo.Add(organizationId.Value.ToString());
                await _cacheService.SetAsync(QuestionCategoryCacheKeys.Version(organizationId.Value), CacheKeyHelper.GenerateVersion());
            }

            await _cacheService.SetAsync(QuestionCategoryCacheKeys.Version(), CacheKeyHelper.GenerateVersion());
            await _notificationHub.Clients.Groups(notifyTo).SendAsync(HubKeys.QuestionCategoryChanged);
        }

        /// <summary>
        /// Retrieves question categories matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A paginated collection of matching question categories.</returns>
        public async Task<PaginationResponse<QuestionCategoryViewModel>> SearchAsync(QuestionCategorySearchInputModel model)
        {
            try
            {
                int? organizationId = _httpContextService.GetOrganizationId();

                model.Normalize();
                model.OrganizationId ??= organizationId;

                string versionKey = QuestionCategoryCacheKeys.Version(model.OrganizationId);
                string? version = await _cacheService.GetAsync<string>(versionKey);

                if (string.IsNullOrWhiteSpace(version))
                {
                    version = CacheKeyHelper.GenerateVersion();

                    await _cacheService.SetAsync(versionKey, version);
                }

                string searchKey = QuestionCategoryCacheKeys.Search(model, version);
                PaginationResponse<QuestionCategoryViewModel>? cacheResult = await _cacheService.GetAsync<PaginationResponse<QuestionCategoryViewModel>>(searchKey);

                if (cacheResult != null)
                {
                    return cacheResult;
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_page_number", model.PageNumber),
                    new("@p_page_size", model.PageSize),
                    new("@p_search_term", model.SearchTerm)
                ];

                IEnumerable<QuestionCategoryModel> result = await _sharedRepository.QueryAsync<QuestionCategoryModel>("[config].[web_get_question_categories]", parameters);

                IEnumerable<QuestionCategoryViewModel> mappedResult = Mapping.Mapper.Map<IEnumerable<QuestionCategoryViewModel>>(result);

                PaginationResponse<QuestionCategoryViewModel> paginationResult = new(result.FirstOrDefault()?.TotalCount ?? 0, mappedResult);

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
        /// Retrieves a question category by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the question category.</param>
        /// <returns>The requested <see cref="QuestionCategoryViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified question category does not exist.</exception>
        public async Task<QuestionCategoryViewModel> GetAsync(int id)
        {
            try
            {
                string idKey = QuestionCategoryCacheKeys.ById(id);

                QuestionCategoryViewModel? cacheResult = await _cacheService.GetAsync<QuestionCategoryViewModel>(idKey);

                if (cacheResult != null)
                {
                    return cacheResult;
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id)
                ];

                QuestionCategoryModel result =
                    await _sharedRepository.QuerySingleAsync<QuestionCategoryModel>("[config].[web_get_question_category_by_id]", parameters)
                    ?? throw new ResponseExceptionFactory(Exceptions.QuestionCategoryNotFound);

                QuestionCategoryViewModel mappedResult = Mapping.Mapper.Map<QuestionCategoryViewModel>(result);

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
        /// Creates a new question category.
        /// </summary>
        /// <param name="model">The question category information.</param>
        /// <returns>The identifier of the newly created question category.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the question category cannot be created.</exception>
        public async Task<int> CreateAsync(QuestionCategoryInputModel model)
        {
            try
            {
                ValidateModel(model);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_name", model.CategoryName),
                    new("@p_description", model.CategoryDescription),
                    new("@p_created_by", _httpContextService.GetUserId()),
                ];

                QuestionCategoryModel result =
                   await _sharedRepository.QuerySingleAsync<QuestionCategoryModel>("[config].[web_insert_question_category]", parameters)
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
        /// Updates an existing question category.
        /// </summary>
        /// <param name="id">The identifier of the question category to update.</param>
        /// <param name="model">The updated question category information.</param>
        public async Task UpdateAsync(int id, QuestionCategoryInputModel model)
        {
            try
            {
                ValidateModel(model);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id),
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_name", model.CategoryName),
                    new("@p_description", model.CategoryDescription),
                    new("@p_updated_by", _httpContextService.GetUserId()),
                ];

                await _sharedRepository.ExecuteAsync("[config].[web_update_question_category]", parameters);

                await HandleChangedAsync(model.OrganizationId, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(UpdateAsync));
                throw;
            }
        }

        /// <summary>
        /// Deletes an existing question category.
        /// </summary>
        /// <param name="id">The identifier of the question category to delete.</param>
        public async Task DeleteAsync(int id)
        {
            try
            {
                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_id", id),
                    new("@p_updated_by", _httpContextService.GetUserId()),
                ];

                QuestionCategoryModel result =
                   await _sharedRepository.QuerySingleAsync<QuestionCategoryModel>("[config].[web_delete_question_category]", parameters)
                    ?? throw new ResponseExceptionFactory(Exceptions.InternalServerError);

                await HandleChangedAsync(result.OrganizationId, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(DeleteAsync));
                throw;
            }
        }
    }
}
