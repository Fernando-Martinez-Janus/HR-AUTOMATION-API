namespace HR_AUTOMATION.Application.ViewModels
{
    public class SearchResultViewModel
    {
        public int SearchRequestId { get; set; }
        public string? CandidateName { get; set; }
        public string? CandidateTitle { get; set; }
        public string? Experience { get; set; }
        public string? CurrentCompany { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Source { get; set; }
        public int AiScore { get; set; }
        public bool AiRecommended { get; set; }
        public string? AiShortComment { get; set; }
        public string? AiExtendedComment { get; set; }
        public string? ReferenceLink { get; set; }
        public string? OriginalResumeLink { get; set; }
        public bool Relocation { get; set; }
        public bool Seen { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}