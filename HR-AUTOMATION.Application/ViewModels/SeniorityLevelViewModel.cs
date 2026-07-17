namespace HR_AUTOMATION.Application.ViewModels
{
    public class SeniorityLevelViewModel
    {
        public int SeniorityLevelId { get; set; }
        public int OrganizationId { get; set; }
        public string SeniorityName { get; set; } = null!;
        public int SortOrder { get; set; }
    }
}
