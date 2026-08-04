using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Entities
{
    [Table("search_results")] // Opcional: ajusta el nombre de la tabla si difiere
    public class SearchResult
    {
        [Column("search_result_id")]
        public int Id { get; set; }

        [Column("search_request_id")]
        public int SearchRequestId { get; set; }

        [Column("candidate_name")]
        public string? CandidateName { get; set; }

        [Column("candidate_title")]
        public string? CandidateTitle { get; set; }

        [Column("experience")]
        public string? Experience { get; set; }

        [Column("current_company")]
        public string? CurrentCompany { get; set; }

        [Column("location")]
        public string? Location { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("source")]
        public string? Source { get; set; }

        [Column("preferences")]
        public string? RawPreferences { get; set; }

        [Column("ia_score")]
        public int? IaScore { get; set; }

        [Column("ia_recommended")]
        public bool IaRecommended { get; set; }

        [Column("ia_short_comment")]
        public string? IaShortComment { get; set; }

        [Column("ia_extended_comment")]
        public string? IaExtendedComment { get; set; }

        [Column("reference_link")]
        public string? ReferenceLink { get; set; }

        [Column("original_resume_link")]
        public string? OriginalResumeLink { get; set; }

        [Column("reubication_flag")]
        public bool Relocation { get; set; }

        [Column("seen")]
        public bool Seen { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("updated_by")]
        public int? UpdatedBy { get; set; }
    }
}