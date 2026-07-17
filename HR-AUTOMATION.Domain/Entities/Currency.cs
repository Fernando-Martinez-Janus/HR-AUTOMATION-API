using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Entities
{
    public class Currency
    {
        [Column("currency_id")]
        public int Id { get; set; }

        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Column("currency_code")]
        public string CurrencyCode { get; set; } = null!;

        [Column("currency_name")]
        public string CurrencyName { get; set; } = null!;

        [Column("currency_symbol")]
        public string? CurrencySymbol { get; set; }

        [Column("sort_order")]
        public int SortOrder { get; set; }

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