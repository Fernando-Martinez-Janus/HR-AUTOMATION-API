namespace HR_AUTOMATION.Application.ViewModels
{
    public class SearchRequestViewModel
    {
        public int SearchRequestId { get; set; }
        public int VacancyId { get; set; }
        public string? VacancyTitle { get; set; }
        public string? ClientName { get; set; }
        public string? VacancyLocation { get; set; }
        public int? MinimumExperience { get; set; }
        public int? MaximumExperience { get; set; }
        public int? ScolarityId { get; set; }
        public string? ScolarityName { get; set; }
        public string? Excluded { get; set; }
        public string? Included { get; set; }
        public string? SearchStatus { get; set; }
        public int TotalRecords { get; set; }
    }
}
