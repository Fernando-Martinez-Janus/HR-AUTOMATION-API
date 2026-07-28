namespace HR_AUTOMATION.Application.ViewModels
{
    public class SearchRequestDispatchViewModel
    {
        public int SearchRequestId { get; set; }
        public int VacancyId { get; set; }
        public string? VacancyTitle { get; set; }
        public string? ClientName { get; set; }
        public string? ProjectName { get; set; }
        public string? VacancyLocation { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public int? CriticalityLevelId { get; set; }
        public string? CriticalityLevelName { get; set; }
        public int? CriticalitySortOrder { get; set; }
        public int ProfileId { get; set; }
        public string? ProfileName { get; set; }
        public int? SeniorityLevelId { get; set; }
        public string? SeniorityLevelName { get; set; }
        public int? AreaLevelId { get; set; }
        public string? AreaLevelName { get; set; }
        public string? WorkModality { get; set; }
        public string? EmploymentType { get; set; }
        public decimal SalaryRangeMin { get; set; }
        public decimal SalaryRangeMax { get; set; }
        public int MaxProfileAgeDays { get; set; }
        public int RequestCooldownMs { get; set; }
        public int? MinimumExperience { get; set; }
        public int? MaximumExperience { get; set; }
        public int? ScolarityId { get; set; }
        public string? ScolarityName { get; set; }
        public IEnumerable<SkillProfileItem>? SkillsProfile { get; set; }
        public string? Excluded { get; set; }
        public string? Included { get; set; }
        public IEnumerable<string>? Sources { get; set; }
        public string? SearchStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<PreviousCandidateViewModel>? PreviousCandidates { get; set; }
    }
}
