namespace HR_AUTOMATION.Domain.Models;

/// <summary>
/// Domain model representing the association between a profile and a skill.
/// </summary>
public class ProfileSkillModel
{
    public int ProfileSkillId { get; set; }
    public int ProfileId { get; set; }
    public int SkillId { get; set; }
    public string? SkillName { get; set; }
    public int SkillLevelId { get; set; }
    public string? SkillLevelName { get; set; }
    public bool IsRequired { get; set; }
    public bool IsEnabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
}