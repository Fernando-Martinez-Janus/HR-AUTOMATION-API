namespace HR_AUTOMATION.Application.ViewModels
{
    public class SkillLevelViewModel
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string? OrganizationName { get; set; }

        public int? TotalRecords { get; set; }
        public string SkillLevelName { get; set; } = string.Empty;
        public string SkillLevelDescription { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }

}
