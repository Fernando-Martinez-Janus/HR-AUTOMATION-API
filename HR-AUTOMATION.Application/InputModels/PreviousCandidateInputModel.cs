namespace HR_AUTOMATION.Application.InputModels;

/// <summary>
/// Represents a candidate that was already evaluated in a previous run of the same search request,
/// so the scraper can skip it instead of downloading and validating it again.
/// </summary>
public class PreviousCandidateInputModel
{
    /// <summary>
    /// Gets or sets the candidate's title as shown on the job portal.
    /// </summary>
    public string CandidateTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the link used to identify this candidate's profile on the job portal.
    /// </summary>
    public string? ReferenceLink { get; set; }
}