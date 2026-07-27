using FluentValidation.Results;
using HR_AUTOMATION.Application.Cache;
using HR_AUTOMATION.Application.Hubs;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.Validators;
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
                "[recruitment].[web_insert_search_result_test]",
                parameters
            );

            // 3. Mapear los resultados (usando el 'Id' heredado de SearchResult)
            List<int> insertedIds = result.Select(r => r.Id).ToList();

            if (!insertedIds.Any())
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
}
