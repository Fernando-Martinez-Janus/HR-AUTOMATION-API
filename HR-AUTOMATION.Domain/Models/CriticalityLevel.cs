namespace HR_AUTOMATION.Domain.Models
{
    public class CriticalityLevel
    {
        public int CriticalityLevelId { get; set; }
        public int OrganizationId { get; set; }
        public string? OrganizationName { get; set; }
        public string CriticalityLevelName { get; set; } = string.Empty;
        public string CriticalityLevelDescription { get; set; } = string.Empty;
        public int SortOrder { get; set; }

        public int? TotalRecords { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
