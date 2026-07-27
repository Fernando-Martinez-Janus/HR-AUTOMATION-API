namespace HR_AUTOMATION.Application.ViewModels
{
    public class SearchResultsViewModel
    {
        public int SearchResultId { get; set; }
        public int SearchRequestId { get; set; }
        public string CandidateTitle { get; set; } = string.Empty;
        public decimal? IaScore { get; set; }
        public string? IaShortComment { get; set; }
        public string? IaExtendedComment { get; set; }
        public string? ReferenceLink { get; set; }
        public string? OriginalResumeLink { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}