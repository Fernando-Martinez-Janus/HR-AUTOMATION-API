namespace HR_AUTOMATION.Application.ViewModels;

/// <summary>
/// View model representing skill information associated with a profile for API responses.
/// </summary>
public class ProfileSkillViewModel
{
    public int ProfileSkillId { get; set; }
    public int ProfileId { get; set; }
    public int SkillId { get; set; }
    public string? SkillName { get; set; }
    public int SkillLevelId { get; set; }
    public string? SkillLevelName { get; set; }
    public bool IsRequired { get; set; }
}