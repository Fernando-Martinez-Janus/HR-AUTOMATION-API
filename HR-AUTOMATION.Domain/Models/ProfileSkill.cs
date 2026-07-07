using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Models
{
    public class ProfileSkill
    {
        [Column("profile_skill_id")]
        public int Id { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Column("profile_id")]
        public int ProfileId { get; set; }

        [Column("skill_id")]
        public int SkillId { get; set; }

        [Column("skill_level_id")]
        public int SkillLevelId { get; set; }

        [Column("skill_weight")]
        public string SkillWeight { get; set; } = string.Empty;

        [Column("skill_name")]
        public string SkillName { get; set; } = string.Empty;

        [Column("level_name")]
        public string LevelName { get; set; } = string.Empty;

        [Column("category_name")]
        public string CategoryName { get; set; } = string.Empty;

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("created_by")]
        public int? CreatedBy { get; set; }

        [Column("updated_by")]
        public int? UpdatedBy { get; set; }

        [Column("total_records")]
        public int TotalRecords { get; set; }
    }
}
