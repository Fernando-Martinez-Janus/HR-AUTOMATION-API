namespace HR_AUTOMATION_WEB_AGENT;

/// <summary>
/// Provides configuration keys used by the polling worker.
/// </summary>
public static class WorkerConstants
{
    /// <summary>
    /// Configuration key for how often, in milliseconds, the worker checks for a pending search request.
    /// </summary>
    public const string PollingIntervalMsKey = "Worker:PollingIntervalMs";

    /// <summary>
    /// Configuration key for the URL that returns the next pending search request to process.
    /// </summary>
    public const string DispatchNextUrlKey = "Worker:DispatchNextUrl";

    /// <summary>
    /// Configuration key for the job portal account email used to log in.
    /// </summary>
    public const string ScraperEmailKey = "Scraper:Credentials:Email";

    /// <summary>
    /// Configuration key for the job portal account password used to log in.
    /// </summary>
    public const string ScraperPasswordKey = "Scraper:Credentials:Password";
}