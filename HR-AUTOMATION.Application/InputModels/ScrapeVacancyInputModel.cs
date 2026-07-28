namespace HR_AUTOMATION.Application.InputModels;

/// <summary>
/// Represents the vacancy data to publish on the job portal.
/// </summary>
public class ScrapeVacancyInputModel
{
    /// <summary>
    /// Gets or sets the job title.
    /// </summary>
    public string JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the work modality (e.g. remote, on-site, hybrid).
    /// </summary>
    public string WorkModality { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employment type.
    /// </summary>
    public string EmploymentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vacancy location.
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the project name.
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the client name.
    /// </summary>
    public string ClientName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the minimum required experience.
    /// </summary>
    public string MinExperience { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the maximum required experience.
    /// </summary>
    public string MaxExperience { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the required education level.
    /// </summary>
    public string EducationLevel { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the keywords used to match candidates.
    /// </summary>
    public string Keywords { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the minimum salary range.
    /// </summary>
    public decimal? MinSalary { get; set; }

    /// <summary>
    /// Gets or sets the maximum salary range.
    /// </summary>
    public decimal? MaxSalary { get; set; }

    /// <summary>
    /// Cleans and normalizes the input values by trimming string properties.
    /// </summary>
    public void Normalize()
    {
        JobTitle = JobTitle.Trim();
        WorkModality = WorkModality.Trim();
        EmploymentType = EmploymentType.Trim();
        Location = Location.Trim();
        ProjectName = ProjectName.Trim();
        ClientName = ClientName.Trim();
        MinExperience = MinExperience.Trim();
        MaxExperience = MaxExperience.Trim();
        EducationLevel = EducationLevel.Trim();
        Keywords = Keywords.Trim();
    }
}