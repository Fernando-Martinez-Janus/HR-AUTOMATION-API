using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Entities
{
    public class ProfileSkill
    {
        [Column("profile_skill_id")]
        public int Id { get; set; }

        [Column("profile_id")]
        public int ProfileId { get; set; }

        [Column("skill_id")]
        public int SkillId { get; set; }

        [Column("skill_level_id")]
        public int SkillLevelId { get; set; }

        [Column("is_required")]
        public bool IsRequired { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("created_by")]
        public int? CreatedBy { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("updated_by")]
        public int? UpdatedBy { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }
    }
}