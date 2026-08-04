using HR_AUTOMATION.Application.Cache;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.Mapper;
using HR_AUTOMATION.Application.ViewModels;
using HR_AUTOMATION.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Kernel.IRepositories;
using Shared.Kernel.Utils.Constants;
using Shared.Kernel.IServices;
using Shared.Kernel.Utils.Helpers;

namespace HR_AUTOMATION.Application.Services
{
    public class SearchRequestService(
        ILogger<SearchRequestService> logger,
        ISharedRepository sharedRepository,
        ICacheService cacheService,
        IConfiguration configuration,
        IHttpContextService httpContextService
    ) : ISearchRequestService
    {
        private readonly ILogger<SearchRequestService> _logger = logger;
        private readonly ISharedRepository _sharedRepository = sharedRepository;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IHttpContextService _httpContextService = httpContextService;

        private readonly TimeSpan _cacheDefaultExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisDefaultExpiration));

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
    }
}
