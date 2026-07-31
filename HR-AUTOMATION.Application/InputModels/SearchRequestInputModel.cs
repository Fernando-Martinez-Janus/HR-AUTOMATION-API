namespace HR_AUTOMATION.Application.InputModels
{
    public class SearchRequestInputModel
    {
        public int? OrganizationId { get; set; }
        public int VacancyId { get; set; }
        public int? MinimumExperience { get; set; }
        public int? MaximumExperience { get; set; }
        public int? ScolarityId { get; set; }
        public int? SeniorityLevelId { get; set; }
        public int? AreaLevelId { get; set; }
        public int? MaxProfileAgeDays { get; set; }
        public string? Excluded { get; set; }
        public string? Included { get; set; }

        public void Normalize()
        {
            Excluded = string.IsNullOrWhiteSpace(Excluded) ? null : Excluded.Trim();
            Included = string.IsNullOrWhiteSpace(Included) ? null : Included.Trim();
        }
    }
}
