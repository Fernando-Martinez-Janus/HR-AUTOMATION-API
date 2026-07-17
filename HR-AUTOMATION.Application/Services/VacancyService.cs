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
    /// Provides business logic for managing vacancies.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="sharedRepository">The shared repository instance.</param>
    /// <param name="cacheService">The cache service instance.</param>
    /// <param name="configuration">The application configuration provider.</param>
    /// <param name="httpContextService">The HTTP context service instance.</param>
    /// <param name="notificationHub">The SignalR notification hub context.</param>
    public class VacancyService(
        ILogger<VacancyService> logger,
        ISharedRepository sharedRepository,
        ICacheService cacheService,
        IConfiguration configuration,
        IHttpContextService httpContextService,
        IHubContext<NotificationHub> notificationHub
    ) : IVacancyService
    {
        private readonly ILogger<VacancyService> _logger = logger;
        private readonly ISharedRepository _sharedRepository = sharedRepository;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IHttpContextService _httpContextService = httpContextService;
        private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;

        private readonly TimeSpan _cacheDefaultExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisDefaultExpiration));

        private readonly TimeSpan _cacheLongExpiration =
            TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisLongExpiration));

        /// <summary>
        /// Normalizes and validates the input model before processing.
        /// Throws a validation exception if the model is invalid.
        /// </summary>
        private void ValidateModel(VacancyInputModel model)
        {
            int? organizationId = _httpContextService.GetOrganizationId();

            model.Normalize();
            model.OrganizationId ??= organizationId;

            ValidationResult validationResult = new VacancyValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                ValidationFailure validationError = validationResult.Errors.First();
                Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);

                throw new ResponseExceptionFactory(currentException);
            }
        }

        /// <summary>
        /// Updates the cache version and notifies the appropriate SignalR groups that vacancies have changed.
        /// </summary>
        /// <param name="organizationId">The organization identifier to notify. If <see langword="null"/>, only the global notification is sent.</param>
        /// <param name="id">The vacancy Id to delete from cache (optional).</param>
        private async Task HandleChangedAsync(int? organizationId = null, int? id = null)
        {
            if (id.HasValue)
            {
                string idKey = VacancyCacheKeys.ById(id.Value);

                await _cacheService.DeleteAsync(idKey);
            }

            List<string> notifyTo = [HubConstants.NotificationAllOrganizationsGroup];

            if (organizationId.HasValue)
            {
                notifyTo.Add(organizationId.Value.ToString());
                await _cacheService.SetAsync(VacancyCacheKeys.Version(organizationId.Value), CacheKeyHelper.GenerateVersion());
            }

            await _cacheService.SetAsync(VacancyCacheKeys.Version(), CacheKeyHelper.GenerateVersion());
            await _notificationHub.Clients.Groups(notifyTo).SendAsync(HubKeys.VacancyChanged);
        }

        /// <summary>
        /// Retrieves vacancies matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A paginated collection of matching vacancies.</returns>
        public async Task<PaginationResponse<VacancyViewModel>> SearchAsync(VacancySearchInputModel model)
        {
            try
            {
                int? organizationId = _httpContextService.GetOrganizationId();

                model.Normalize();
                model.OrganizationId ??= organizationId;

                string versionKey = VacancyCacheKeys.Version(model.OrganizationId);
                string? version = await _cacheService.GetAsync<string>(versionKey);

                if (string.IsNullOrWhiteSpace(version))
                {
                    version = CacheKeyHelper.GenerateVersion();

                    await _cacheService.SetAsync(versionKey, version);
                }

                string searchKey = VacancyCacheKeys.Search(model, version);
                PaginationResponse<VacancyViewModel>? cacheResult = await _cacheService.GetAsync<PaginationResponse<VacancyViewModel>>(searchKey);

                if (cacheResult != null)
                {
                    return cacheResult;
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_organization_id", model.OrganizationId),
                    new("@page_number", model.PageNumber),
                    new("@rows_page", model.PageSize)
                ];

                IEnumerable<Vacancy> result = await _sharedRepository.QueryAsync<Vacancy>("[recruitment].[web_get_vacancies]", parameters);

                IEnumerable<VacancyViewModel> mappedResult = Mapping.Mapper.Map<IEnumerable<VacancyViewModel>>(result);

                PaginationResponse<VacancyViewModel> paginationResult = new(result.FirstOrDefault()?.TotalRecords ?? 0, mappedResult);

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
        /// Retrieves a vacancy by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the vacancy.</param>
        /// <returns>The requested <see cref="VacancyViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified vacancy does not exist.</exception>
        public async Task<VacancyViewModel> GetAsync(int id)
        {
            try
            {
                string idKey = VacancyCacheKeys.ById(id);

                VacancyViewModel? cacheResult = await _cacheService.GetAsync<VacancyViewModel>(idKey);

                if (cacheResult != null)
                {
                    return cacheResult;
                }

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_vacancy_id", id)
                ];

                Vacancy result =
                    await _sharedRepository.QuerySingleAsync<Vacancy>("[recruitment].[web_get_vacancy_by_id]", parameters)
                    ?? throw new ResponseExceptionFactory(Exceptions.VacancyNotFound);

                VacancyViewModel mappedResult = Mapping.Mapper.Map<VacancyViewModel>(result);

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
        /// Creates a new vacancy.
        /// </summary>
        /// <param name="model">The vacancy information.</param>
        /// <returns>The identifier of the newly created vacancy.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the vacancy cannot be created.</exception>
        public async Task<int> CreateAsync(VacancyInputModel model)
        {
            try
            {
                ValidateModel(model);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_profile_id", model.ProfileId),
                    new("@p_criticality_level_id", model.CriticalityLevelId),
                    new("@p_vacancy_status_id", model.VacancyStatusId),
                    new("@p_vacancy_title", model.VacancyTitle),
                    new("@p_client_name", model.ClientName),
                    new("@p_project_name", model.ProjectName),
                    new("@p_vacancy_location", model.VacancyLocation),
                    new("@p_position_count", model.PositionCount),
                    new("@p_salary_range_min", model.SalaryRangeMin),
                    new("@p_salary_range_max", model.SalaryRangeMax),
                    new("@p_request_date", model.RequestDate),
                    new("@p_deadline_date", model.DeadlineDate),
                    new("@p_modality_id", model.ModalityId),
                    new("@p_contract_type_id", model.ContractTypeId),
                    new("@p_currency_id", model.CurrencyId),
                    new("@p_pay_frequency_id", model.PayFrequencyId),
                    new("@p_notes", model.Notes),
                    new("@p_created_by", _httpContextService.GetUserId())
                ];

                object? result = await _sharedRepository.QueryScalarAsync("[recruitment].[web_ins_vacancy]", parameters);

                await HandleChangedAsync(model.OrganizationId);

                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CreateAsync));
                throw;
            }
        }

        /// <summary>
        /// Creates or updates a vacancy (draft). If the vacancy identifier is provided, the existing vacancy is updated; otherwise, a new one is created.
        /// </summary>
        /// <param name="model">The vacancy information.</param>
        /// <returns>The <see cref="VacancyViewModel"/> of the upserted vacancy.</returns>
        public async Task<VacancyViewModel> UpsertAsync(VacancyInputModel model)
        {
            try
            {
                ValidateModel(model);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_vacancy_id", model.VacancyId),
                    new("@p_organization_id", model.OrganizationId),
                    new("@p_profile_id", model.ProfileId),
                    new("@p_criticality_level_id", model.CriticalityLevelId),
                    new("@p_vacancy_status_id", model.VacancyStatusId),
                    new("@p_vacancy_title", model.VacancyTitle),
                    new("@p_client_name", model.ClientName),
                    new("@p_project_name", model.ProjectName),
                    new("@p_vacancy_location", model.VacancyLocation),
                    new("@p_position_count", model.PositionCount),
                    new("@p_salary_range_min", model.SalaryRangeMin),
                    new("@p_salary_range_max", model.SalaryRangeMax),
                    new("@p_request_date", model.RequestDate),
                    new("@p_deadline_date", model.DeadlineDate),
                    new("@p_modality_id", model.ModalityId),
                    new("@p_contract_type_id", model.ContractTypeId),
                    new("@p_currency_id", model.CurrencyId),
                    new("@p_pay_frequency_id", model.PayFrequencyId),
                    new("@p_notes", model.Notes),
                    new("@p_created_by", _httpContextService.GetUserId()),
                    new("@p_updated_by", _httpContextService.GetUserId())
                ];

                object? scalarResult = await _sharedRepository.QueryScalarAsync("[recruitment].[web_upsert_vacancy]", parameters);

                int vacancyId = Convert.ToInt32(scalarResult);

                List<KeyValuePair<string, object?>> fetchParams = [
                    new("@p_vacancy_id", vacancyId)
                ];

                Vacancy item =
                    await _sharedRepository.QuerySingleAsync<Vacancy>("[recruitment].[web_get_vacancy_by_id]", fetchParams)
                    ?? throw new ResponseExceptionFactory(Exceptions.InternalServerError);

                VacancyViewModel mappedResult = Mapping.Mapper.Map<VacancyViewModel>(item);

                await _cacheService.SetAsync(VacancyCacheKeys.ById(vacancyId), mappedResult, _cacheLongExpiration);

                await HandleChangedAsync(model.OrganizationId);

                return mappedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(UpsertAsync));
                throw;
            }
        }

        /// <summary>
        /// Updates an existing vacancy.
        /// </summary>
        /// <param name="id">The identifier of the vacancy to update.</param>
        /// <param name="model">The updated vacancy information.</param>
        public async Task UpdateAsync(int id, VacancyInputModel model)
        {
            try
            {
                ValidateModel(model);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_vacancy_id", id),
                    new("@p_profile_id", model.ProfileId),
                    new("@p_criticality_level_id", model.CriticalityLevelId),
                    new("@p_vacancy_status_id", model.VacancyStatusId),
                    new("@p_vacancy_title", model.VacancyTitle),
                    new("@p_client_name", model.ClientName),
                    new("@p_project_name", model.ProjectName),
                    new("@p_vacancy_location", model.VacancyLocation),
                    new("@p_position_count", model.PositionCount),
                    new("@p_salary_range_min", model.SalaryRangeMin),
                    new("@p_salary_range_max", model.SalaryRangeMax),
                    new("@p_request_date", model.RequestDate),
                    new("@p_deadline_date", model.DeadlineDate),
                    new("@p_modality_id", model.ModalityId),
                    new("@p_contract_type_id", model.ContractTypeId),
                    new("@p_currency_id", model.CurrencyId),
                    new("@p_pay_frequency_id", model.PayFrequencyId),
                    new("@p_notes", model.Notes),
                    new("@p_updated_by", _httpContextService.GetUserId())
                ];

                await _sharedRepository.ExecuteAsync("[recruitment].[web_upd_vacancy]", parameters);

                await HandleChangedAsync(model.OrganizationId, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(UpdateAsync));
                throw;
            }
        }

        /// <summary>
        /// Deletes an existing vacancy.
        /// </summary>
        /// <param name="id">The identifier of the vacancy to delete.</param>
        public async Task DeleteAsync(int id)
        {
            try
            {
                List<KeyValuePair<string, object?>> fetchParams = [
                    new("@p_vacancy_id", id)
                ];

                Vacancy existing =
                    await _sharedRepository.QuerySingleAsync<Vacancy>("[recruitment].[web_get_vacancy_by_id]", fetchParams)
                    ?? throw new ResponseExceptionFactory(Exceptions.VacancyNotFound);

                List<KeyValuePair<string, object?>> parameters = [
                    new("@p_vacancy_id", id),
                    new("@p_updated_by", _httpContextService.GetUserId())
                ];

                await _sharedRepository.ExecuteAsync("[recruitment].[web_del_vacancy]", parameters);

                await HandleChangedAsync(existing.OrganizationId, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(DeleteAsync));
                throw;
            }
        }
    }
}
