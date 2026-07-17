namespace HR_AUTOMATION.Application.ViewModels
{
    public class SkillLevelViewModel
    {
        public int SkillLevelId { get; set; }
        public int OrganizationId { get; set; }
        public string LevelName { get; set; } = null!;
        public string LevelDescription { get; set; } = null!;
        public int SortOrder { get; set; }
    }
}
