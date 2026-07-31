using HR_AUTOMATION.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Models;

public class ProfileSkillModel : ProfileSkill
{
    [Column("level_name")]
    public string? SkillLevelName { get; set; }

    [Column("skill_name")]
    public string? SkillName { get; set; }
}
