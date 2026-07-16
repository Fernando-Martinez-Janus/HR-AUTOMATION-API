namespace HR_AUTOMATION.Application.ViewModels
{
    public class SkillCategoryViewModel
    {
        public int SkillCategoryId { get; set; }
        public int OrganizationId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? IconName { get; set; }
        public int SortOrder { get; set; }
    }
}
