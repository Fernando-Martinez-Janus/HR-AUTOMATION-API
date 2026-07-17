using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Entities
{
    public class RejectionReason
    {
        [Column("rejection_reason_id")]
        public int Id { get; set; }

        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Column("reason_description")]
        public string Description { get; set; } = null!;

        [Column("is_definitive")]
        public bool IsDefinitive { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("created_by")]
        public int? CreatedBy { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("updated_by")]
        public int UpdatedBy { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }
    }
}