namespace HR_AUTOMATION.Application.InputModels;

/// <summary>
/// Represents the data required to create a new profile.
/// </summary>
public class ProfileInputModel
{


    /// <summary>
    /// Gets or sets the organization identifier.
    /// </summary>
    public int? OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the area level identifier.
    /// </summary>
    public int? AreaLevelId { get; set; }

    /// <summary>
    /// Gets or sets the seniority level identifier.
    /// </summary>
    public int? SeniorityLevelId { get; set; }

    /// <summary>
    /// Gets or sets the profile name.
    /// </summary>
    public string ProfileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the profile description.
    /// </summary>
    public string ProfileDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the profile icon name.
    /// </summary>
    public string? IconName { get; set; }

    /// <summary>
    /// Gets or sets the profile color.
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the list of skills to associate with this profile.
    /// </summary>
    public IEnumerable<ProfileSkillInputModel> Skills { get; set; } = [];

    /// <summary>
    /// Cleans and normalizes the input values by trimming string properties.
    /// </summary>
    public void Normalize()
    {
        ProfileName = ProfileName.Trim();
        ProfileDescription = ProfileDescription.Trim();
    }
}
