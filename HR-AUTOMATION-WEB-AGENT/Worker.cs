using System.Text.Json;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.ViewModels;
using Shared.Kernel.InputModels;
using Shared.Kernel.IServices;
using Shared.Kernel.Responses;
using Shared.Kernel.ViewModels;

namespace HR_AUTOMATION_WEB_AGENT;

/// <summary>
/// Periodically checks for a pending search request and, when one is found, runs a CV scraping job for it.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="configuration">The application configuration provider.</param>
/// <param name="httpService">The HTTP service used to call the dispatch endpoint.</param>
/// <param name="scraperService">The service that performs the actual CV scraping.</param>
public class Worker(
    ILogger<Worker> logger,
    IConfiguration configuration,
    IHttpService httpService,
    IScraperService scraperService
) : BackgroundService
{
    /// <summary>
    /// Used for logging error and information messages.
    /// </summary>
    private readonly ILogger<Worker> _logger = logger;

    /// <summary>
    /// Used to call the dispatch endpoint that returns the next pending search request.
    /// </summary>
    private readonly IHttpService _httpService = httpService;

    /// <summary>
    /// Performs the actual CV scraping for a search request.
    /// </summary>
    private readonly IScraperService _scraperService = scraperService;

    /// <summary>
    /// The URL that returns the next pending search request to process.
    /// </summary>
    private readonly string _dispatchNextUrl = configuration.GetValue<string>(WorkerConstants.DispatchNextUrlKey)!;

    /// <summary>
    /// How often to check for a pending search request.
    /// </summary>
    private readonly TimeSpan _pollingInterval =
        TimeSpan.FromMilliseconds(configuration.GetValue<long>(WorkerConstants.PollingIntervalMsKey));

    /// <summary>
    /// The job portal account email used to log in.
    /// </summary>
    private readonly string _scraperEmail = configuration.GetValue<string>(WorkerConstants.ScraperEmailKey)!;

    /// <summary>
    /// The job portal account password used to log in.
    /// </summary>
    private readonly string _scraperPassword = configuration.GetValue<string>(WorkerConstants.ScraperPasswordKey)!;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(_pollingInterval);

        do
        {
            await CheckForPendingSearchRequestAsync();
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }

    /// <summary>
    /// Asks the dispatch endpoint for the next pending search request and, if there is one, runs the scrape for it.
    /// </summary>
    private async Task CheckForPendingSearchRequestAsync()
    {
        try
        {
            HttpRequest dispatchRequest = new()
            {
                Method = HttpMethod.Get,
                Url = _dispatchNextUrl
            };

            HttpResponse httpResponse = await _httpService.SendRequestAsync(dispatchRequest);
            Response<List<SearchRequestDispatchViewModel>>? response = JsonSerializer.Deserialize<Response<List<SearchRequestDispatchViewModel>>>(
                httpResponse.GetResponseAsString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (response?.DataResponse is null or { Count: 0 })
            {
                _logger.LogInformation("No pending search requests");
                return;
            }

            foreach (SearchRequestDispatchViewModel dispatch in response.DataResponse)
            {
                Guid jobId = Guid.NewGuid();

                _logger.LogInformation("Search request {SearchRequestId} received, starting scrape (job {JobId})", dispatch.SearchRequestId, jobId);

                try
                {
                    ScrapeInputModel scrapeInput = BuildScrapeInputModel(dispatch);

                    await _scraperService.ScrapeAsync(scrapeInput, jobId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Scrape failed for search request {SearchRequestId} (job {JobId})", dispatch.SearchRequestId, jobId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CheckForPendingSearchRequestAsync));
        }
    }

    /// <summary>
    /// Maps the dispatch information for a search request into the data <see cref="IScraperService.ScrapeAsync"/> needs.
    /// </summary>
    /// <remarks>
    /// <see cref="ScrapeCredentialsInputModel"/> is filled from this worker's own configuration, not from the
    /// dispatch response, since login credentials for the job portal aren't part of a search request's data.
    /// </remarks>
    private ScrapeInputModel BuildScrapeInputModel(SearchRequestDispatchViewModel dispatch)
    {
        string keywords = dispatch.SkillsProfile is null
            ? string.Empty
            : string.Join(", ", dispatch.SkillsProfile.Select(skill => skill.SkillName));

        IEnumerable<PreviousCandidateInputModel> previousCandidates = dispatch.PreviousCandidates?.Select(candidate =>
            new PreviousCandidateInputModel
            {
                CandidateTitle = candidate.CandidateTitle,
                ReferenceLink = candidate.ReferenceLink
            }) ?? [];

        return new ScrapeInputModel
        {
            Credentials = new ScrapeCredentialsInputModel
            {
                Email = _scraperEmail,
                Password = _scraperPassword
            },
            SearchCriteria = new CvSearchInputModel
            {
                JobTitle = dispatch.VacancyTitle ?? string.Empty,
                Location = dispatch.VacancyLocation,
                EmploymentType = dispatch.EmploymentType,
                EducationLevel = dispatch.ScolarityName,
                MinSalary = dispatch.SalaryRangeMin,
                MaxSalary = dispatch.SalaryRangeMax,
                MaxProfileAgeDays = dispatch.MaxProfileAgeDays,
                IncludedKeywords = dispatch.Included,
                ExcludedKeywords = dispatch.Excluded,
                PreviousCandidates = previousCandidates
            },
            Vacancy = new ScrapeVacancyInputModel
            {
                JobTitle = dispatch.VacancyTitle ?? string.Empty,
                WorkModality = dispatch.WorkModality ?? string.Empty,
                EmploymentType = dispatch.EmploymentType ?? string.Empty,
                Location = dispatch.VacancyLocation ?? string.Empty,
                ProjectName = dispatch.ProjectName ?? string.Empty,
                ClientName = dispatch.ClientName ?? string.Empty,
                MinExperience = dispatch.MinimumExperience?.ToString() ?? string.Empty,
                MaxExperience = dispatch.MaximumExperience?.ToString() ?? string.Empty,
                EducationLevel = dispatch.ScolarityName ?? string.Empty,
                Keywords = keywords,
                MinSalary = dispatch.SalaryRangeMin,
                MaxSalary = dispatch.SalaryRangeMax
            }
        };
    }
}