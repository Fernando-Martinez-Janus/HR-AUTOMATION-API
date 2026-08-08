namespace HR_AUTOMATION.Application.ViewModels
{
    public class SearchResultViewModel
    {
        public int SearchResultId { get; set; }
        public int SearchRequestId { get; set; }
        public int VacancyId { get; set; }
        public string? CandidateName { get; set; }
        public string? CandidateTitle { get; set; }
        public string? Experience { get; set; }
        public string? CurrentCompany { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Source { get; set; }

        public List<string> Preferences { get; set; } = [];

        public int AiScore { get; set; }
        public bool AiRecommended { get; set; }
        public string? AiShortComment { get; set; }
        public string? AiExtendedComment { get; set; }
        public string? ReferenceLink { get; set; }
        public string? OriginalResumeLink { get; set; }
        public bool Relocation { get; set; }
        public bool Seen { get; set; }
        public int CandidateRank { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? HrRanking { get; set; }
        public string? HrComments { get; set; }
    }
}