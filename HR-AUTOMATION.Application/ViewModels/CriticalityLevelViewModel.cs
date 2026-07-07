
namespace HR_AUTOMATION.Application.ViewModels
{
    public class CriticalityLevelViewModel
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string? OrganizationName { get; set; }

        public int? TotalRecords { get; set; }
        public string CriticalityLevelName { get; set; } = string.Empty;
        public string CriticalityLevelDescription { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }

}
