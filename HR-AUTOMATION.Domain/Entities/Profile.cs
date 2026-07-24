using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Entities
{
    public class Profile
    {
        [Column("profile_id")]
        public int Id { get; set; }

        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Column("area_level_id")]
        public int AreaLevelId { get; set; }

        [Column("seniority_level_id")]
        public int SeniorityLevelId { get; set; }

        [Column("profile_name")]
        public string ProfileName { get; set; } = null!;

        [Column("profile_description")]
        public string? ProfileDescription { get; set; }

        [Column("icon_name")]
        public string? IconName { get; set; }

        [Column("color")]
        public string? Color { get; set; }

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