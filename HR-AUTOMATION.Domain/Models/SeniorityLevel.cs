using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Models
{
    public class SeniorityLevel
    {
        [Column("seniority_level_id")]
        public int Id { get; set; }

        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Column("seniority_name")]
        public string SeniorityName { get; set; } = string.Empty;

        [Column("organization_name")]
        public string? OrganizationName { get; set; }

        [Column("sort_order")]
        public int SortOrder { get; set; }

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
