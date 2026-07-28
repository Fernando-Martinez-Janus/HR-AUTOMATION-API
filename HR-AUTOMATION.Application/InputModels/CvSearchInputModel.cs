namespace HR_AUTOMATION.Application.InputModels;

/// <summary>
/// Represents the filter criteria used to search for candidate CVs on the job portal.
/// </summary>
public class CvSearchInputModel
{
    /// <summary>
    /// Gets or sets the job title to search for.
    /// </summary>
    public string JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the candidate location filter.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Gets or sets the employment type filter.
    /// </summary>
    public string? EmploymentType { get; set; }

    /// <summary>
    /// Gets or sets the education level filter.
    /// </summary>
    public string? EducationLevel { get; set; }

    /// <summary>
    /// Gets or sets the minimum salary filter.
    /// </summary>
    public decimal? MinSalary { get; set; }

    /// <summary>
    /// Gets or sets the maximum salary filter.
    /// </summary>
    public decimal? MaxSalary { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of CVs to collect.
    /// </summary>
    public int MaxCvs { get; set; } = 100;

    /// <summary>
    /// Gets or sets the number of CVs to process per page.
    /// </summary>
    public int CvsPerPage { get; set; } = 20;

    /// <summary>
    /// Gets or sets the maximum number of days since the candidate's profile/CV was last updated.
    /// Profiles updated longer ago than this are discarded.
    /// </summary>
    public int? MaxProfileAgeDays { get; set; }

    /// <summary>
    /// Gets or sets the candidates already evaluated in a previous run of this search request, so
    /// they can be skipped instead of being downloaded and validated again.
    /// </summary>
    public IEnumerable<PreviousCandidateInputModel> PreviousCandidates { get; set; } = [];

    /// <summary>
    /// Cleans and normalizes the input values by trimming string properties.
    /// </summary>
    public void Normalize()
    {
        JobTitle = JobTitle.Trim();
        Location = Location?.Trim();
        EmploymentType = EmploymentType?.Trim();
        EducationLevel = EducationLevel?.Trim();
    }
}