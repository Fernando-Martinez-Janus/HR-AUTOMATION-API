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
using Newtonsoft.Json;
using Shared.Kernel.IRepositories;
using Shared.Kernel.IServices;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Constants;
using Shared.Kernel.Utils.Enums;
using Shared.Kernel.Utils.Helpers;
using Shared.Kernel.Utils.Json;
using Shared.Kernel.ViewModels;

namespace HR_AUTOMATION.Application.Services;

/// <summary>
/// Handles profile management operations.
/// </summary>
/// <param name="logger">Logger instance.</param>
/// <param name="sharedRepository">Shared database repository.</param>
/// <param name="cacheService">Cache service.</param>
/// <param name="configuration">Application configuration.</param>
/// <param name="httpContextService">HTTP context service.</param>
/// <param name="notificationHub">SignalR notification hub context.</param>
public class ProfileService(
    ILogger<ProfileService> logger,
    ISharedRepository sharedRepository,
    ICacheService cacheService,
    IConfiguration configuration,
    IHttpContextService httpContextService,
    IHubContext<NotificationHub> notificationHub) : IProfileService
{
    private readonly ILogger<ProfileService> _logger = logger;
    private readonly ISharedRepository _sharedRepository = sharedRepository;
    private readonly ICacheService _cacheService = cacheService;
    private readonly IHttpContextService _httpContextService = httpContextService;
    private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;

    private readonly TimeSpan _cacheDefaultExpiration =
        TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisDefaultExpiration));

    private readonly TimeSpan _cacheLongExpiration =
        TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisLongExpiration));

    private readonly JsonSerializerSettings _jsonSettings = new()
    {
        ContractResolver = new ColumnAttributeContractResolver()
    };

    /// <summary>
    /// Normalizes and validates the input model before processing.
    /// Throws a validation exception if the model is invalid.
    /// </summary>
    private void ValidateModel(ProfileInputModel model)
    {
        int? organizationId = _httpContextService.GetOrganizationId();

        model.Normalize();
        model.OrganizationId ??= organizationId;

        ValidationResult validationResult = new ProfileValidator().Validate(model);

        if (!validationResult.IsValid)
        {
            ValidationFailure validationError = validationResult.Errors.First();
            Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);

            throw new ResponseExceptionFactory(currentException);
        }
    }

    /// <summary>
    /// Updates the cache version and notifies the appropriate SignalR groups that profiles have changed.
    /// </summary>
    /// <param name="organizationId">The organization identifier to notify.</param>
    /// <param name="id">The profile identifier to delete from cache (optional).</param>
    private async Task HandleChangedAsync(int? organizationId = null, int? id = null)
    {
        if (id.HasValue)
        {
            string idKey = ProfileCacheKeys.ById(id.Value);
            await _cacheService.DeleteAsync(idKey);
        }

        List<string> notifyTo = [HubConstants.NotificationAllOrganizationsGroup];

        if (organizationId.HasValue)
        {
            notifyTo.Add(organizationId.Value.ToString());
            await _cacheService.SetAsync(ProfileCacheKeys.Version(organizationId.Value), CacheKeyHelper.GenerateVersion());
        }

        await _cacheService.SetAsync(ProfileCacheKeys.Version(), CacheKeyHelper.GenerateVersion());
        await _notificationHub.Clients.Groups(notifyTo).SendAsync(HubKeys.ProfileChanged);
    }

    /// <summary>
    /// Retrieves profiles matching the specified search criteria.
    /// </summary>
    /// <param name="model">The search criteria.</param>
    /// <returns>A collection of matching profiles.</returns>
    public async Task<PaginationResponse<ProfileViewModel>> SearchAsync(ProfileSearchInputModel model)
    {
        try
        {
            int? organizationId = _httpContextService.GetOrganizationId();

            model.Normalize();
            model.OrganizationId ??= organizationId;

            string versionKey = SkillCacheKeys.Version(model.OrganizationId);
            string? version = await _cacheService.GetAsync<string>(versionKey);

            if (string.IsNullOrWhiteSpace(version))
            {
                version = CacheKeyHelper.GenerateVersion();

                await _cacheService.SetAsync(versionKey, version);
            }

            string searchKey = ProfileCacheKeys.Search(model, version);

            PaginationResponse<ProfileViewModel>? cacheResult = await _cacheService.GetAsync<PaginationResponse<ProfileViewModel>>(searchKey);

            if (cacheResult != null)
            {
                return cacheResult;
            }

            List<KeyValuePair<string, object?>> parameters = [
                new("p_organization_id", model.OrganizationId),
                new("@p_page_number", model.PageNumber),
                new("@p_page_size", model.PageSize),
                new("@p_area_level_id", model.AreaLevelId),
                new("@p_seniority_level_id", model.SeniorityLevelId),
                new("@p_search_term", model.SearchTerm)
            ];

            IEnumerable<ProfileModel> result =
                await _sharedRepository.QueryAsync<ProfileModel>("[recruitment].[web_get_profiles]", parameters);

            IEnumerable<ProfileViewModel> mappedResult = Mapping.Mapper.Map<IEnumerable<ProfileViewModel>>(result);

            PaginationResponse<ProfileViewModel> paginationResult = new(result.FirstOrDefault()?.TotalCount ?? 0, mappedResult);

            await _cacheService.SetAsync(searchKey, mappedResult, _cacheDefaultExpiration);

            return paginationResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(SearchAsync));
            throw;
        }
    }

    /// <summary>
    /// Retrieves a profile by its identifier.
    /// </summary>
    /// <param name="id">The profile identifier.</param>
    /// <returns>The profile information.</returns>
    /// <exception cref="ResponseExceptionFactory">Thrown when the profile is not found.</exception>
    public async Task<ProfileViewModel> GetAsync(int id)
    {
        try
        {
            string idKey = ProfileCacheKeys.ById(id);

            ProfileViewModel? cacheResult = await _cacheService.GetAsync<ProfileViewModel>(idKey);

            if (cacheResult != null)
            {
                return cacheResult;
            }

            List<KeyValuePair<string, object?>> parameters = [
                new("@p_profile_id", id)
            ];

            ProfileModel result =
                await _sharedRepository.QuerySingleAsync<ProfileModel>("[recruitment].[web_get_profile_by_id]", parameters)
                ?? throw new ResponseExceptionFactory(Exceptions.ProfileNotFound);

            ProfileViewModel mappedResult = Mapping.Mapper.Map<ProfileViewModel>(result);

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
    /// Creates a new profile with its associated skills using JSON parameter.
    /// </summary>
    /// <param name="model">The profile information.</param>
    /// <returns>The identifier of the newly created profile.</returns>
    /// <exception cref="ResponseExceptionFactory">Thrown when the profile cannot be created.</exception>
    public async Task<int> CreateAsync(ProfileInputModel model)
    {
        try
        {
            ValidateModel(model);

            IEnumerable<ProfileSkillModel> skills = Mapping.Mapper.Map<IEnumerable<ProfileSkillModel>>(model.Skills);

            List<KeyValuePair<string, object?>> parameters = [
                new("@p_organization_id",     model.OrganizationId),
                new("@p_area_level_id",       model.AreaLevelId),
                new("@p_seniority_level_id",  model.SeniorityLevelId),
                new("@p_profile_name",        model.ProfileName),
                new("@p_profile_description", model.ProfileDescription),
                new("@p_skills",              JsonConvert.SerializeObject(skills, _jsonSettings)),
                new("@p_created_by",          _httpContextService.GetUserId())
            ];

            ProfileModel result =
                await _sharedRepository.QuerySingleAsync<ProfileModel>("[recruitment].[web_insert_profile]", parameters)
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
    /// Updates an existing profile.
    /// </summary>
    /// <param name="id">The profile identifier.</param>
    /// <param name="model">The updated profile information.</param>
    public async Task UpdateAsync(int id, ProfileInputModel model)
    {
        try
        {
            ValidateModel(model);

            List<KeyValuePair<string, object?>> parameters = [
                new("@p_profile_id",          id),
                new("@p_organization_id",     model.OrganizationId),
                new("@p_area_level_id",       model.AreaLevelId),
                new("@p_seniority_level_id",  model.SeniorityLevelId),
                new("@p_profile_name",        model.ProfileName),
                new("@p_profile_description", model.ProfileDescription),
                new("@p_skills",              JsonConvert.SerializeObject(model.Skills, _jsonSettings)),
                new("@p_updated_by",          _httpContextService.GetUserId())
            ];

            await _sharedRepository.ExecuteAsync("[recruitment].[web_update_profile]", parameters);

            await HandleChangedAsync(model.OrganizationId, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(UpdateAsync));
            throw;
        }
    }
}