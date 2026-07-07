using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Models
{
    public class Permission
    {
        [Column("permission_id")]
        public long Id { get; set; }

        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Column("permission_name")]
        public string PermissionName { get; set; } = string.Empty;

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
