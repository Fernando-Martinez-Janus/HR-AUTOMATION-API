using System.Text.Json;
using System.Text.RegularExpressions;
using HR_AUTOMATION.Application.Constants;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Shared.Kernel.InputModels;
using Shared.Kernel.IServices;
using Shared.Kernel.ViewModels;

namespace HR_AUTOMATION.Application.Services;

/// <summary>
/// Provides browser automation to log in to an external job portal, search for candidate CVs
/// matching a vacancy, validate each candidate against the vacancy requirements, and download the matching CVs.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="configuration">The application configuration provider.</param>
/// <param name="httpService">The HTTP service used to call the candidate validation model.</param>
public class ScraperService(
    ILogger<ScraperService> logger,
    IConfiguration configuration,
    IHttpService httpService
) : IScraperService
{
    /// <summary>
    /// Used for logging error and information messages.
    /// </summary>
    private readonly ILogger<ScraperService> _logger = logger;

    /// <summary>
    /// Used to call the local model that validates candidates against the vacancy requirements.
    /// </summary>
    private readonly IHttpService _httpService = httpService;

    /// <summary>
    /// The job portal's login page URL.
    /// </summary>
    private readonly string _loginUrl = configuration.GetValue<string>(ScraperConstants.LoginUrlKey)!;

    /// <summary>
    /// The local folder where downloaded CVs are stored.
    /// </summary>
    private readonly string _downloadPath = configuration.GetValue<string>(ScraperConstants.DownloadPathKey)!;

    /// <summary>
    /// The base address of the Ollama server used to validate candidates.
    /// </summary>
    private readonly string _ollamaBaseUrl = configuration.GetValue<string>(ScraperConstants.OllamaBaseUrlKey)!;

    /// <summary>
    /// The Ollama model used to validate candidates.
    /// </summary>
    private readonly string _ollamaModel = configuration.GetValue<string>(ScraperConstants.OllamaModelKey)!;

    /// <summary>
    /// The URL used to report found candidates back to the main API.
    /// </summary>
    private readonly string _saveResultUrl = configuration.GetValue<string>(ScraperConstants.SaveResultUrlKey)!;

    /// <summary>
    /// The Playwright instance used to drive the browser. Lazily created on the first scrape.
    /// </summary>
    private IPlaywright? _playwright;

    /// <summary>
    /// The browser instance used to run scraping jobs. Lazily created on the first scrape.
    /// </summary>
    private IBrowser? _browser;

    /// <summary>
    /// Runs a full scraping job: logs in to the job portal, searches for candidate CVs matching
    /// <paramref name="request"/>, validates each candidate and downloads the matching CVs.
    /// </summary>
    /// <param name="request">The scraping job data, including credentials, search criteria and vacancy information.</param>
    /// <param name="jobId">The identifier of the scraping job, used to correlate log entries for this run.</param>
    public async Task ScrapeAsync(ScrapeInputModel request, Guid jobId)
    {
        using IDisposable? logScope = _logger.BeginScope(new Dictionary<string, object> { ["JobId"] = jobId });

        try
        {
            _playwright ??= await Playwright.CreateAsync();
            _browser ??= await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });

            Directory.CreateDirectory(_downloadPath);

            string vacancyFilePath = Path.Combine(_downloadPath, "active_vacancy.json");
            string vacancyJson = JsonSerializer.Serialize(request.Vacancy, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(vacancyFilePath, vacancyJson);
            _logger.LogInformation("Vacancy data saved to {VacancyFilePath}", vacancyFilePath);

            IBrowserContext context = await _browser.NewContextAsync();

            try
            {
                await RunScrapeJobAsync(context, request);
            }
            finally
            {
                await context.CloseAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(ScrapeAsync));
            throw;
        }
    }

    /// <summary>
    /// Logs in to the job portal, paginates through the search results and downloads the CVs of
    /// candidates that pass both the recency and the model validation checks.
    /// </summary>
    private async Task RunScrapeJobAsync(IBrowserContext context, ScrapeInputModel request)
    {
        IPage mainPage = await context.NewPageAsync();

        await mainPage.GotoAsync(_loginUrl);

        await mainPage.FillAsync("#Email", request.Credentials.Email);
        await Task.Delay(1000);
        await mainPage.FillAsync("#Password", request.Credentials.Password);
        await Task.Delay(1000);

        await mainPage.ClickAsync("#submitBtn");

        await mainPage.WaitForSelectorAsync("#Hirercenter_dashboard_headertalento", new() { State = WaitForSelectorState.Hidden });
        await Task.Delay(1000);
        await mainPage.ClickAsync("#Hirercenter_dashboard_headertalento");

        await mainPage.WaitForSelectorAsync("#Searchpage_Puesto", new() { State = WaitForSelectorState.Hidden });
        await Task.Delay(1000);
        await mainPage.FillAsync("#Searchpage_Puesto", request.SearchCriteria.JobTitle);
        await Task.Delay(1000);
        await mainPage.ClickAsync("#SearchPage_Buscar");

        await mainPage.WaitForSelectorAsync("#results-page", new() { State = WaitForSelectorState.Hidden });
        await Task.Delay(5000);

        await mainPage.Locator("a.srp\\|results\\|list").First.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
            Timeout = 30000
        });

        int totalLinks = await mainPage.Locator("a.srp\\|results\\|list").CountAsync();
        _logger.LogInformation("Found {TotalLinks} links to process", totalLinks);

        if (totalLinks == 0)
        {
            throw new InvalidOperationException("No result links found on the page.");
        }

        int processed = 0;
        int accessed = 0;
        int page = 1;
        bool hasNext = true;

        List<SearchResultCandidateInputModel> savedCandidates = [];

        HashSet<string> processedLinks = request.SearchCriteria.PreviousCandidates
            .Select(candidate => candidate.ReferenceLink)
            .Where(referenceLink => !string.IsNullOrEmpty(referenceLink))
            .ToHashSet()!;

        _logger.LogInformation("Excluding {PreviousCandidatesCount} previously evaluated candidates", processedLinks.Count);

        while (hasNext && accessed < request.SearchCriteria.MaxCvs)
        {
            _logger.LogInformation("Processing page {Page}", page);

            ILocator allLinks = mainPage.Locator("a.srp\\|results\\|list");
            int linksOnPage = await allLinks.CountAsync();
            _logger.LogInformation("Found {LinksOnPage} links on this page", linksOnPage);

            if (linksOnPage == 0)
            {
                _logger.LogInformation("No links found to process on this page. Exiting");
                break;
            }

            for (int i = 0; i < linksOnPage; i++)
            {
                if (accessed >= request.SearchCriteria.MaxCvs)
                {
                    _logger.LogInformation("Valid links target achieved");
                    break;
                }

                if (i >= request.SearchCriteria.CvsPerPage)
                {
                    _logger.LogInformation("Reached the {CvsPerPage} elements limit on this page. Skipping the rest", request.SearchCriteria.CvsPerPage);
                    break;
                }

                processed++;

                ILocator individualLink = allLinks.Nth(i);
                _logger.LogInformation("[Global: {Processed} | Valid: {Accessed}] Processing link {Index} of page {Page}", processed, accessed, i + 1, page);

                ILocator cardInfo = individualLink
                    .Locator("> div")
                    .Locator("> div").Nth(1)
                    .Locator("> div").First;

                ILocator titleParagraph = cardInfo.Locator("p").First;
                ILocator dateParagraph = cardInfo.Locator("> p").First;

                int dateCount = await dateParagraph.CountAsync();

                if (dateCount <= 0)
                {
                    _logger.LogWarning("No date paragraph found for this candidate card");
                    continue;
                }

                string dateText = await dateParagraph.InnerTextAsync();
                _logger.LogInformation("Date paragraph text captured: '{DateText}'", dateText);

                string candidateTitle = await titleParagraph.CountAsync() > 0
                    ? await titleParagraph.InnerTextAsync()
                    : string.Empty;

                if (!await IsWithinMaxProfileAgeAsync(dateText, request.SearchCriteria.MaxProfileAgeDays))
                {
                    _logger.LogInformation("Discarded: date '{DateText}' is older than the allowed profile age", dateText);
                    continue;
                }

                string? url = await individualLink.GetAttributeAsync("href");

                if (string.IsNullOrEmpty(url))
                {
                    continue;
                }

                if (!processedLinks.Add(url))
                {
                    _logger.LogInformation("Skipped: profile '{Url}' was already processed on a previous page", url);
                    continue;
                }

                _logger.LogInformation("Match: '{DateText}' passed the profile age check. Opening profile", dateText);

                IPage newTab = await mainPage.Context.NewPageAsync();
                await newTab.GotoAsync(url);
                await newTab.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                await newTab.WaitForLoadStateAsync(LoadState.NetworkIdle);

                string profileText = await newTab.Locator("body").InnerTextAsync();

                if (!await ValidateCandidateAsync(profileText, request.Vacancy, request.SearchCriteria))
                {
                    _logger.LogInformation("Candidate not eligible. Skipping");
                    await newTab.CloseAsync();
                    continue;
                }

                accessed++;

                Task<IDownload> downloadTask = newTab.WaitForDownloadAsync(new() { Timeout = 60000 });
                await newTab.Locator("use[*|href='#atomic__download']").ClickAsync();
                IDownload download = await downloadTask;
                string targetPath = Path.Combine(_downloadPath, download.SuggestedFilename);
                await download.SaveAsAsync(targetPath);
                _logger.LogInformation("CV saved to {TargetPath}", targetPath);

                savedCandidates.Add(new SearchResultCandidateInputModel
                {
                    CandidateTitle = candidateTitle,
                    ReferenceLink = url,
                    OriginalResumeLink = download.Url
                });

                await Task.Delay(1000);
                await newTab.CloseAsync();
            }

            if (accessed >= request.SearchCriteria.MaxCvs)
            {
                break;
            }

            await Task.Delay(3000);

            ILocator nextButton = mainPage.Locator("#Resultpage_PaginadorPaginaSiguiente");

            if (!await nextButton.IsVisibleAsync() || !await nextButton.IsEnabledAsync())
            {
                _logger.LogInformation("No more pages available");
                hasNext = false;
                continue;
            }

            await nextButton.ClickAsync();
            await Task.Delay(2000);

            await mainPage.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await mainPage.Locator("a.srp\\|results\\|list").First.WaitForAsync(new()
            {
                State = WaitForSelectorState.Visible,
                Timeout = 15000
            });
            await mainPage.WaitForLoadStateAsync(LoadState.NetworkIdle);
            page++;
        }

        await Task.Delay(30000);

        _logger.LogInformation("Scraping job completed. Processed {Processed}, downloaded {Downloaded}", processed, accessed);

        if (savedCandidates.Count > 0)
        {
            await SaveResultsAsync(request.SearchRequestId, savedCandidates);
        }
    }

    /// <summary>
    /// Reports the candidates found in this scrape run back to the main API so they're persisted
    /// and excluded from future runs of the same search request via previous-candidate deduplication.
    /// </summary>
    private async Task SaveResultsAsync(int searchRequestId, List<SearchResultCandidateInputModel> candidates)
    {
        try
        {
            HttpRequest saveRequest = new()
            {
                Method = HttpMethod.Post,
                Url = _saveResultUrl,
                Body = new SearchResultsInputModel
                {
                    SearchRequestId = searchRequestId,
                    Candidates = candidates
                }
            };

            HttpResponse saveResponse = await _httpService.SendRequestAsync(saveRequest);

            if ((int)saveResponse.Status is < 200 or >= 300)
            {
                _logger.LogError(
                    "Failed to save {Count} search results for search request {SearchRequestId}. Status: {Status}. Body: {Body}",
                    candidates.Count, searchRequestId, saveResponse.Status, saveResponse.GetResponseAsString());
                return;
            }

            _logger.LogInformation("Saved {Count} search results for search request {SearchRequestId}", candidates.Count, searchRequestId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save search results for search request {SearchRequestId}", searchRequestId);
        }
    }

    /// <summary>
    /// Asks the local Ollama model whether the candidate profile matches the vacancy requirements.
    /// Search criteria values take precedence over the vacancy defaults when provided.
    /// </summary>
    /// <param name="profileText">The candidate's profile page text.</param>
    /// <param name="vacancy">The vacancy requirements.</param>
    /// <param name="searchCriteria">The search criteria used to override the vacancy defaults.</param>
    /// <returns><see langword="true"/> if the candidate matches the requirements, or if the validation call fails; otherwise, <see langword="false"/>.</returns>
    private async Task<bool> ValidateCandidateAsync(string profileText, ScrapeVacancyInputModel vacancy, CvSearchInputModel searchCriteria)
    {
        string location = searchCriteria.Location ?? vacancy.Location;
        decimal? minSalary = searchCriteria.MinSalary ?? vacancy.MinSalary;
        decimal? maxSalary = searchCriteria.MaxSalary ?? vacancy.MaxSalary;
        string educationLevel = searchCriteria.EducationLevel ?? vacancy.EducationLevel;
        string employmentType = searchCriteria.EmploymentType ?? "Any";
        string includedKeywords = searchCriteria.IncludedKeywords ?? "None";
        string excludedKeywords = searchCriteria.ExcludedKeywords ?? "None";

        string prompt = $@"
        You are a fast recruiter checking candidates against job requirements.
        For each item answer ONLY 'YES' or 'NO'.
        If the candidate's profile doesn't explicitly show the required info, answer 'NO'.

        [JOB REQUIREMENTS]
        - Role: {vacancy.JobTitle}
        - Work mode: {vacancy.WorkModality}
        - Location: {location}
        - Contract type: {employmentType}
        - Experience: {vacancy.MinExperience}-{vacancy.MaxExperience} years
        - Education: {educationLevel}
        - Key skills: {vacancy.Keywords}
        - Must include: {includedKeywords}
        - Must NOT include: {excludedKeywords}
        - Salary range: {minSalary} - {maxSalary}

        [CANDIDATE PROFILE]
        {profileText}

        [CHECKLIST]
        1. Does the role match or relate to '{vacancy.JobTitle}'? _
        2. Is the work mode '{vacancy.WorkModality}' compatible? _
        3. Does the location '{location}' match? _
        4. Is the contract type '{employmentType}' compatible? (skip if 'Any') _
        5. Is experience between {vacancy.MinExperience} and {vacancy.MaxExperience} years? _
        6. Does education meet '{educationLevel}'? _
        7. Does the candidate have the required key skills? _
        8. Does the profile show '{includedKeywords}'? (skip if 'None') _
        9. Does the profile avoid '{excludedKeywords}'? (skip if 'None') _
        10. Is the expected salary within {minSalary} - {maxSalary}? _

        If ALL are 'YES', answer 'SI'. If ANY is 'NO', answer 'NO'.
        ANSWER (only 'SI' or 'NO'):";

        try
        {
            return await AskOllamaAsync(prompt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ollama validation failed. Proceeding with default download");
            return true;
        }
    }

    /// <summary>
    /// Asks the local Ollama model whether the given relative date phrase (e.g. "hace 3 días", "3 days ago")
    /// falls within the allowed profile age. The phrase can be in any language, since it is read directly
    /// from the job portal's UI and the portal's language depends on the browser locale of the machine
    /// running the scrape.
    /// </summary>
    /// <param name="text">The relative date text shown on the job portal.</param>
    /// <param name="maxProfileAgeDays">
    /// The maximum number of days since the profile was last updated. When <see langword="null"/>,
    /// the search request has no age requirement, so this check is skipped entirely.
    /// </param>
    /// <returns><see langword="true"/> if the date is within the allowed age, if no age limit was set, or if the validation call fails; otherwise, <see langword="false"/>.</returns>
    private async Task<bool> IsWithinMaxProfileAgeAsync(string text, int? maxProfileAgeDays)
    {
        if (maxProfileAgeDays is not int maxAgeDays)
        {
            return true;
        }

        string prompt = $@"
        You are analyzing a relative date phrase shown on a job portal, describing how long ago a
        candidate profile was last updated. The phrase can be written in any language.

        Determine whether it describes a moment within the last {maxAgeDays} days.
        Answer ONLY 'SI' if it is within that period, or 'NO' if it is older.

        Phrase: '{text}'
        ANSWER (only 'SI' or 'NO'):";

        try
        {
            return await AskOllamaAsync(prompt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ollama date validation failed. Assuming the profile is recent");
            return true;
        }
    }

    /// <summary>
    /// Sends a yes/no prompt to the local Ollama model and returns whether it answered "SI".
    /// </summary>
    /// <remarks>
    /// The model tends to reason through the checklist before giving its final answer, so instead of
    /// requiring the raw response to be exactly "SI", this takes the last standalone "SI"/"NO" it wrote.
    /// </remarks>
    private async Task<bool> AskOllamaAsync(string prompt)
    {
        HttpRequest ollamaRequest = new()
        {
            Method = HttpMethod.Post,
            Url = $"{_ollamaBaseUrl}/api/generate",
            Body = new
            {
                model = _ollamaModel,
                prompt,
                stream = false,
                options = new { temperature = 0.1, num_predict = 200 }
            }
        };

        HttpResponse ollamaResponse = await _httpService.SendRequestAsync(ollamaRequest);
        JsonElement json = JsonSerializer.Deserialize<JsonElement>(ollamaResponse.Response);
        string rawAnswer = json.GetProperty("response").GetString() ?? string.Empty;

        _logger.LogInformation("Ollama response: {RawAnswer}", rawAnswer);

        MatchCollection matches = Regex.Matches(rawAnswer.ToUpperInvariant(), @"\b(SI|NO)\b");
        string? answer = matches.Count > 0 ? matches[^1].Value : null;

        return answer == "SI";
    }
}