namespace HR_AUTOMATION.Domain.Models
{
    public class AreaLevel
    {
        public int AreaLevelId { get; set; }
        public int OrganizationId { get; set; }
        public string? OrganizationName { get; set; }
        public string AreaLevelName { get; set; } = string.Empty;
        public string AreaLevelDescription { get; set; } = string.Empty;
        public int SortOrder { get; set; }

        public int? TotalRecords { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}

