using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Entities
{
    public class Organization
    {
        [Column("organization_id")]
        public int Id { get; set; }

        [Column("organization_name")]
        public string Name { get; set; } = null!;

        [Column("slug")]
        public string Slug { get; set; } = null!;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }
    }
}