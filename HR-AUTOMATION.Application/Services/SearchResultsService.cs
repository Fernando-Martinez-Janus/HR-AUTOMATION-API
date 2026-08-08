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
using System.Text.Json;

namespace HR_AUTOMATION.Application.Services;

public class SearchResultsService(
        ILogger<SearchResultsService> logger,
        ISharedRepository sharedRepository,
        ICacheService cacheService,
        IConfiguration configuration,
        IHttpContextService httpContextService,
        IHubContext<NotificationHub> notificationHub,
        IHttpService httpService
    ) : ISearchResultsService
{
    private readonly ILogger<SearchResultsService> _logger = logger;
    private readonly ISharedRepository _sharedRepository = sharedRepository;
    private readonly ICacheService _cacheService = cacheService;
    private readonly IHttpContextService _httpContextService = httpContextService;
    private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;
    private readonly IHttpService _httpService = httpService;

    private readonly TimeSpan _cacheDefaultExpiration =
        TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisDefaultExpiration));

    private readonly TimeSpan _cacheLongExpiration =
        TimeSpan.FromMilliseconds(configuration.GetValue<long>(AppConstants.RedisLongExpiration));

    private void ValidateModel(SearchResultsInputModel model)
    {
        int? organizationId = _httpContextService.GetOrganizationId();

        model.Normalize();
        model.OrganizationId ??= organizationId;

        ValidationResult validationResult = new SearchResultsValidator().Validate(model);

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
            await _cacheService.SetAsync(SearchResultsCacheKeys.Version(organizationId.Value), CacheKeyHelper.GenerateVersion());
        }

        await _cacheService.SetAsync(SearchResultsCacheKeys.Version(), CacheKeyHelper.GenerateVersion());
        await _notificationHub.Clients.Groups(notifyTo).SendAsync(HubKeys.SearchResultsChanged);
    }

    public async Task<IEnumerable<int>> CreateAsync(SearchResultsInputModel model)
    {
        try
        {
            ValidateModel(model);

            // 1. Serializar candidatos si JsonQuery viene nulo
            string? jsonQuery = !string.IsNullOrWhiteSpace(model.JsonQuery)
                ? model.JsonQuery
                : (model.Candidates != null ? JsonSerializer.Serialize(model.Candidates) : null);

            if (string.IsNullOrWhiteSpace(jsonQuery))
            {
                throw new ResponseExceptionFactory(Exceptions.jsonQueryRequired);
            }

            // Asigna un valor por defecto para pruebas si userId viene nulo:
            int? userId = _httpContextService.GetUserId();
            int createdBy = userId ?? 1;


            List<KeyValuePair<string, object?>> parameters = [
                new("@p_search_request_id", model.SearchRequestId),
                new("@p_link_perfil", model.LinkPerfil),
                new("@p_json_query", jsonQuery),
                new("@p_created_by", createdBy)
            ];

            // 2. Ejecutar el Stored Procedure usando el tipo de retorno adecuado
            IEnumerable<SearchResultModel> result = await _sharedRepository.QueryAsync<SearchResultModel>(
                "[recruitment].[web_insert_search_result]",
                parameters
            );

            // 3. Mapear los resultados (usando el 'Id' heredado de SearchResult)
            List<int> insertedIds = result.Select(r => r.Id).ToList();

            // Un scraper puede reportar una corrida completa sin candidatos a propósito (para que
            // el SP marque la vacante como "sin candidatos"), así que solo es un error real si
            // se esperaban inserciones y no se generó ninguna.
            if (model.Candidates is { Count: > 0 } && !insertedIds.Any())
            {
                throw new ResponseExceptionFactory(Exceptions.InternalServerError);
            }

            await HandleChangedAsync(_httpContextService.GetOrganizationId());

            return insertedIds;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CreateAsync));
            throw;
        }
    }

    public async Task<ActiveSearchViewModel> GetByVacancyAsync(int vacancyId, SourcingResultSearchInputModel model)
    {
        try
        {
            model.Normalize();

            List<KeyValuePair<string, object?>> parameters = [
                new("@p_vacancy_id", vacancyId),
                new("@p_page_number", model.PageNumber),
                new("@p_page_size", model.PageSize)
            ];

            IEnumerable<SearchResultModel> result = await _sharedRepository.QueryAsync<SearchResultModel>(
                "[recruitment].[web_get_search_results_vacancy]", parameters
            );

            IEnumerable<SearchResultViewModel> mappedResult = Mapping.Mapper.Map<IEnumerable<SearchResultViewModel>>(result);

            return new ActiveSearchViewModel
            {
                UnSeenCount = result.FirstOrDefault()?.UnseenCount ?? 0,
                Result = new PaginationResponse<SearchResultViewModel>(result.FirstOrDefault()?.TotalRecords ?? 0, mappedResult)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(GetByVacancyAsync));
            throw;
        }
    }

    public async Task<SearchResultViewModel> GetDetailByVacancyAsync(int vacancyId, int resultId)
    {
        try
        {
            List<KeyValuePair<string, object?>> parameters = [
                new("@p_vacancy_id", vacancyId),
                new("@p_search_result_id", resultId)
            ];

            SearchResultModel result =
                await _sharedRepository.QuerySingleAsync<SearchResultModel>("[recruitment].[web_get_search_results_vacancy_detail]", parameters)
                ?? throw new ResponseExceptionFactory(Exceptions.SearchResultNotFound);

            return Mapping.Mapper.Map<SearchResultViewModel>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(GetDetailByVacancyAsync));
            throw;
        }
    }

    public async Task<IEnumerable<SearchResultViewModel>> GetTopCandidatesAsync(int topCount = 3)
    {
        try
        {
            List<KeyValuePair<string, object?>> parameters = [
                new("@p_top", topCount)
            ];

            IEnumerable<TopCandidateSearchResultModel> result = await _sharedRepository.QueryAsync<TopCandidateSearchResultModel>(
                "[recruitment].[web_get_search_results_top_candidates]", parameters
            );

            return Mapping.Mapper.Map<IEnumerable<SearchResultViewModel>>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(GetTopCandidatesAsync));
            throw;
        }
    }
}
