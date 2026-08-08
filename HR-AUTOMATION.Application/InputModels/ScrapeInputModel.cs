namespace HR_AUTOMATION.Application.InputModels;

/// <summary>
/// Represents the data required to run a CV scraping job against an external job portal.
/// </summary>
public class ScrapeInputModel
{
    /// <summary>
    /// Gets or sets the identifier of the search request this scrape job is running for, used to
    /// report found candidates back to the main API.
    /// </summary>
    public int SearchRequestId { get; set; }

    /// <summary>
    /// Gets or sets the credentials used to log in to the job portal.
    /// </summary>
    public ScrapeCredentialsInputModel Credentials { get; set; } = new();

    /// <summary>
    /// Gets or sets the criteria used to search for candidate CVs.
    /// </summary>
    public CvSearchInputModel SearchCriteria { get; set; } = new();

    /// <summary>
    /// Gets or sets the vacancy data to publish on the job portal.
    /// </summary>
    public ScrapeVacancyInputModel Vacancy { get; set; } = new();

    /// <summary>
    /// Cleans and normalizes the input values by trimming string properties.
    /// </summary>
    public void Normalize()
    {
        Credentials.Normalize();
        SearchCriteria.Normalize();
        Vacancy.Normalize();
    }
}