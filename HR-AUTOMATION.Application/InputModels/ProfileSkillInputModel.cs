namespace HR_AUTOMATION.Application.InputModels;

/// <summary>
/// Represents a skill entry to associate with a profile on creation.
/// </summary>
public class ProfileSkillInputModel
{
    /// <summary>
    /// Gets or sets the skill identifier.
    /// </summary>
    public int SkillId { get; set; }

    /// <summary>
    /// Gets or sets the skill level identifier.
    /// </summary>
    public int SkillLevelId { get; set; }

    /// <summary>
    /// Gets or sets whether the skill is required for this profile.
    /// </summary>
    public bool IsRequired { get; set; }
}
