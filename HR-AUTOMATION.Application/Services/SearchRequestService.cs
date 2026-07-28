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
using Shared.Kernel.InputModels;
using Shared.Kernel.IRepositories;
using Shared.Kernel.IServices;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Constants;
using Shared.Kernel.Utils.Enums;
using Shared.Kernel.Utils.Helpers;
using System.Text.Json;

namespace HR_AUTOMATION.Application.Services
{
    public class SearchRequestService(
        ILogger<SearchRequestService> logger,
        ISharedRepository sharedRepository,
        ICacheService cacheService,
        IConfiguration configuration,
        IHttpContextService httpContextService,
        IHubContext<NotificationHub> notificationHub,
        IHttpService httpService
    ) : ISearchRequestService
    {
        private readonly ILogger<SearchRequestService> _logger = logger;
        private readonly ISharedRepository _sharedRepository = sharedRepository;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IHttpContextService _httpContextService = httpContextService;
        private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;
        private readonly IHttpService _httpService = httpService;

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

                string? skillsProfile = null;

                Vacancy? vacancy = await _sharedRepository.QuerySingleAsync<Vacancy>(
                    "[recruitment].[web_get_vacancy_by_id]",
                    [new("@p_vacancy_id", model.VacancyId)]
                );

                if (vacancy != null)
                {
                    IEnumerable<ProfileSkillResultModel> profileSkills = await _sharedRepository.QueryAsync<ProfileSkillResultModel>(
                        "[recruitment].[web_get_profile_skills]",
                        [new("@p_profile_id", vacancy.ProfileId)]
                    );

                    if (profileSkills?.Any() == true)
                    {
                        skillsProfile = JsonSerializer.Serialize(
                            profileSkills.Select(s => new
                            {
                                skillId = s.SkillId,
                                skillCategoryId = s.SkillCategoryId,
                                skillLevelId = s.SkillLevelId
                            })
                        );
                    }
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_vacancy_id", model.VacancyId),
                    new("@p_minimum_experience", model.MinimumExperience),
                    new("@p_maximum_experience", model.MaximumExperience),
                    new("@p_scolarity_id", model.ScolarityId),
                    new("@p_skills_profile", skillsProfile),
                    new("@p_excluded", model.Excluded),
                    new("@p_included", model.Included),
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

        public async Task<int> SendToScraperAsync(ActiveSearchInputModel model)
        {
            try
            {
                int? organizationId = _httpContextService.GetOrganizationId();
                int? userId = _httpContextService.GetUserId();

                model.VacancyId = model.VacancyId;
                model.MinExperience = model.MinExperience;
                model.MaxExperience = model.MaxExperience;

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_vacancy_id", model.VacancyId),
                    new("@p_minimum_experience", model.MinExperience),
                    new("@p_maximum_experience", model.MaxExperience),
                    new("@p_scolarity_id", model.Education),
                    new("@p_profile_json", model.CvUpdated),
                    new("@p_excluded_companies", model.KeywordsExclude),
                    new("@p_excluded_schools", null),
                    new("@p_created_by", userId),
                ];

                SearchRequestModel result =
                    await _sharedRepository.QuerySingleAsync<SearchRequestModel>("[recruitment].[web_insert_search_request]", parameters)
                    ?? throw new ResponseExceptionFactory(Exceptions.InternalServerError);

                await _httpService.SendRequestAsync(new HttpRequest
                {
                    Url = configuration.GetValue<string>("Scraper:Url")!,
                    Method = HttpMethod.Post,
                    Body = new
                    {
                        searchRequestId = result.Id,
                        vacancyId = model.VacancyId,
                        minExperience = model.MinExperience,
                        maxExperience = model.MaxExperience,
                        education = model.Education,
                        cvUpdated = model.CvUpdated,
                        keywordsExclude = model.KeywordsExclude,
                        sources = model.Sources
                    },
                    Timeout = 30000
                });

                await HandleChangedAsync(organizationId);

                return result.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(SendToScraperAsync));
                throw;
            }
        }

        public async Task<SearchRequestDispatchViewModel> GetDispatchAsync(int searchRequestId)
        {
            try
            {
                int? createdBy = _httpContextService.GetUserId();

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_created_by", createdBy),
                    new("@p_vacancy_id", null),
                    new("@p_search_request_id", searchRequestId)
                ];

                SearchRequestDispatchModel result =
                    await _sharedRepository.QuerySingleAsync<SearchRequestDispatchModel>("[recruitment].[web_get_search_request_dispatch]", parameters)
                    ?? throw new ResponseExceptionFactory(Exceptions.InternalServerError);

                SearchRequestDispatchViewModel viewModel = Mapping.Mapper.Map<SearchRequestDispatchViewModel>(result);

                if (!string.IsNullOrWhiteSpace(result.PreviousCandidates))
                {
                    viewModel.PreviousCandidates = JsonSerializer.Deserialize<IEnumerable<PreviousCandidateViewModel>>(
                        result.PreviousCandidates,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }

                if (!string.IsNullOrWhiteSpace(result.SkillsProfile))
                {
                    viewModel.SkillsProfile = JsonSerializer.Deserialize<IEnumerable<SkillProfileItem>>(
                        result.SkillsProfile,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }

                if (!string.IsNullOrWhiteSpace(result.Sources))
                {
                    viewModel.Sources = JsonSerializer.Deserialize<IEnumerable<string>>(
                        result.Sources,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetDispatchAsync));
                throw;
            }
        }
    }
}
