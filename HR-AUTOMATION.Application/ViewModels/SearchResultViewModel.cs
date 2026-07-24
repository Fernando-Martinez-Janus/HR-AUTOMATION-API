namespace HR_AUTOMATION.Application.ViewModels
{
    public class SearchResultViewModel
    {
        public int SearchRequestId { get; set; }
        public string CandidateTitle { get; set; } = string.Empty;
        public int AiScore { get; set; }
        public bool AiRecommended { get; set; }
        public string AiShortComment { get; set; } = string.Empty;
        public string AiExtendedComment { get; set; } = string.Empty;
        public string? ReferenceLink { get; set; }
        public string? OriginalResumeLink { get; set; }
        public bool Seen { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}