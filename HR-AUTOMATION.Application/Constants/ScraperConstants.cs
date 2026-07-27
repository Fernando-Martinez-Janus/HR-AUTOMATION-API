namespace HR_AUTOMATION.Application.Constants;

/// <summary>
/// Provides configuration keys used by the CV scraping service.
/// </summary>
public static class ScraperConstants
{
    /// <summary>
    /// Configuration key for the job portal's login page URL.
    /// </summary>
    public const string LoginUrlKey = "Scraper:LoginUrl";

    /// <summary>
    /// Configuration key for the local folder where downloaded CVs are stored.
    /// </summary>
    public const string DownloadPathKey = "Scraper:DownloadPath";

    /// <summary>
    /// Configuration key for the base address of the Ollama server used to validate candidates.
    /// </summary>
    public const string OllamaBaseUrlKey = "Scraper:Ollama:BaseUrl";

    /// <summary>
    /// Configuration key for the Ollama model used to validate candidates.
    /// </summary>
    public const string OllamaModelKey = "Scraper:Ollama:Model";
}