using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Models
{
    public class UserRole
    {
        [Column("user_role_id")]
        public long Id { get; set; }

        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Column("organization_name")]
        public string? OrganizationName { get; set; }

        [Column("user_id")]
        public long IdUser { get; set; }

        [Column("role_id")]
        public long IdRole { get; set; }

        [Column("role_name")]
        public string? RoleName { get; set; }

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
