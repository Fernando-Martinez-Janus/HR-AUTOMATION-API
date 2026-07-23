using HR_AUTOMATION.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Models;

/// <summary>
/// Domain model representing the association between a profile and a skill.
/// </summary>
public class ProfileSkillModel : ProfileSkill
{
    [Column("level_name")]
    public string? SkillLevelName { get; set; }

    [Column("skill_name")]
    public string? SkillName { get; set; }
}