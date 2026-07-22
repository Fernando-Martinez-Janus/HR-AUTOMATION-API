namespace HR_AUTOMATION.Application.InputModels
{
    public class SearchRequestInputModel
    {
        public int? OrganizationId { get; set; }
        public int VacancyId { get; set; }
        public int? MinimumExperience { get; set; }
        public int? MaximumExperience { get; set; }
        public int? ScolarityId { get; set; }
        public string? ProfileJson { get; set; }
        public string? ExcludedCompanies { get; set; }
        public string? ExcludedSchools { get; set; }

        public void Normalize()
        {
            ProfileJson = string.IsNullOrWhiteSpace(ProfileJson) ? null : ProfileJson.Trim();
            ExcludedCompanies = string.IsNullOrWhiteSpace(ExcludedCompanies) ? null : ExcludedCompanies.Trim();
            ExcludedSchools = string.IsNullOrWhiteSpace(ExcludedSchools) ? null : ExcludedSchools.Trim();
        }
    }
}
