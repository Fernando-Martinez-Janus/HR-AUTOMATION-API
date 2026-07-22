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
    public class SearchRequestService(
        ILogger<SearchRequestService> logger,
        ISharedRepository sharedRepository,
        ICacheService cacheService,
        IConfiguration configuration,
        IHttpContextService httpContextService,
        IHubContext<NotificationHub> notificationHub
    ) : ISearchRequestService
    {
        private readonly ILogger<SearchRequestService> _logger = logger;
        private readonly ISharedRepository _sharedRepository = sharedRepository;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IHttpContextService _httpContextService = httpContextService;
        private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;

        private readonly TimeSpan _cacheDefaultExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisDefaultExpiration));

        private readonly TimeSpan _cacheLongExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisLongExpiration));

        private void ValidateModel(SearchRequestInputModel model)
        {
            int? organizationId = _httpContextService.GetOrganizationId();

            model.Normalize();
            model.OrganizationId ??= organizationId;

            ValidationResult validationResult = new SearchRequestValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                ValidationFailure validationError = validationResult.Errors.First();
                Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);

                throw new ResponseExceptionFactory(currentException);
            }
        }

        private async Task HandleChangedAsync(int? organizationId = null)
        {
            List<string> notifyTo = [HubConstants.NotificationAllOrganizationsGroup];

            if (organizationId.HasValue)
            {
                notifyTo.Add(organizationId.Value.ToString());
                await _cacheService.SetAsync(SearchRequestCacheKeys.Version(organizationId.Value), CacheKeyHelper.GenerateVersion());
            }

            await _cacheService.SetAsync(SearchRequestCacheKeys.Version(), CacheKeyHelper.GenerateVersion());
            await _notificationHub.Clients.Groups(notifyTo).SendAsync(HubKeys.SearchRequestChanged);
        }

        public async Task<IEnumerable<SearchRequestViewModel>> SearchAsync(SearchRequestSearchInputModel model)
        {
            try
            {
                int? organizationId = _httpContextService.GetOrganizationId();

                model.Normalize();
                model.OrganizationId ??= organizationId;

                string versionKey = SearchRequestCacheKeys.Version(model.OrganizationId);
                string? version = await _cacheService.GetAsync<string>(versionKey);

                if (string.IsNullOrWhiteSpace(version))
                {
                    version = CacheKeyHelper.GenerateVersion();
                    await _cacheService.SetAsync(versionKey, version);
                }

                string searchKey = SearchRequestCacheKeys.Search(model, version);
                IEnumerable<SearchRequestViewModel>? cacheResult = await _cacheService.GetAsync<IEnumerable<SearchRequestViewModel>>(searchKey);

                if (cacheResult != null)
                {
                    return cacheResult;
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_page_number", model.PageNumber),
                    new("@p_page_size", model.PageSize)
                ];

                IEnumerable<SearchRequestModel> result = await _sharedRepository.QueryAsync<SearchRequestModel>("[recruitment].[web_get_search_requests]", parameters);

                IEnumerable<SearchRequestViewModel> mappedResult = Mapping.Mapper.Map<IEnumerable<SearchRequestViewModel>>(result);

                await _cacheService.SetAsync(searchKey, mappedResult, _cacheDefaultExpiration);

                return mappedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(SearchAsync));
                throw;
            }
        }

        public async Task<int> CreateAsync(SearchRequestInputModel model)
        {
            try
            {
                ValidateModel(model);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_vacancy_id", model.VacancyId),
                    new("@p_minimum_experience", model.MinimumExperience),
                    new("@p_maximum_experience", model.MaximumExperience),
                    new("@p_scolarity_id", model.ScolarityId),
                    new("@p_profile_json", model.ProfileJson),
                    new("@p_excluded_companies", model.ExcludedCompanies),
                    new("@p_excluded_schools", model.ExcludedSchools),
                    new("@p_created_by", _httpContextService.GetUserId()),
                ];

                SearchRequestModel result =
                    await _sharedRepository.QuerySingleAsync<SearchRequestModel>("[recruitment].[web_insert_search_request]", parameters)
                    ?? throw new ResponseExceptionFactory(Exceptions.InternalServerError);

                await HandleChangedAsync(_httpContextService.GetOrganizationId());

                return result.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CreateAsync));
                throw;
            }
        }
    }
}
