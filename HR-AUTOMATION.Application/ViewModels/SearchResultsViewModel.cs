namespace HR_AUTOMATION.Application.ViewModels
{
    public class SearchResultsViewModel
    {
        public int SearchResultId { get; set; }
        public int SearchRequestId { get; set; }
        public string? CandidateName { get; set; }
        public string? CandidateTitle { get; set; }
        public string? Experience { get; set; }
        public string? CurrentCompany { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Source { get; set; }

        public int? AiScore { get; set; } // Cambiado de decimal? a int? e IaScore -> AiScore
        public bool AiRecommended { get; set; }
        public string? AiShortComment { get; set; }
        public string? AiExtendedComment { get; set; }

        public string? ReferenceLink { get; set; }
        public string? OriginalResumeLink { get; set; }
        public bool Relocation { get; set; }
        public bool Seen { get; set; }

        public bool IsEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}