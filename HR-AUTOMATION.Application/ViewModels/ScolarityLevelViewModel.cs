namespace HR_AUTOMATION.Application.ViewModels
{
    public class ScolarityLevelViewModel
    {
        public int ScolarityLevelId { get; set; }
        public int OrganizationId { get; set; }
        public string LevelName { get; set; } = null!;
        public string? LevelDescription { get; set; }
        public int SortOrder { get; set; }
    }
}
