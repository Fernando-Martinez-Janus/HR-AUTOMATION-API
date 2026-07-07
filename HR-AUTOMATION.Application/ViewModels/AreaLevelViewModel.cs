namespace HR_AUTOMATION.Application.ViewModels
{
    public class AreaLevelViewModel
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string? OrganizationName { get; set; }

        public int? TotalRecords { get; set; }
        public string AreaLevelName { get; set; } = string.Empty;
        public string AreaLevelDescription { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }

}
