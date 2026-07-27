using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Entities
{
    public class SearchResult
    {
        [Column("search_result_id")]
        public int Id { get; set; }

        [Column("search_request_id")]
        public int SearchRequestId { get; set; }

        [Column("candidate_title")]
        public string CandidateTitle { get; set; } = null!;

        [Column("ia_score")]
        public int? IaScore { get; set; } // Change from decimal? to int?

        [Column("ia_short_comment")]
        public string? IaShortComment { get; set; }

        [Column("ia_extended_comment")]
        public string? IaExtendedComment { get; set; }

        [Column("reference_link")]
        public string? ReferenceLink { get; set; }

        [Column("original_resume_link")]
        public string? OriginalResumeLink { get; set; }

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