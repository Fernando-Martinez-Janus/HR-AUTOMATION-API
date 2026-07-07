namespace HR_AUTOMATION.Domain.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }
        public int OrganizationId { get; set; }
        public string? OrganizationName { get; set; }
        public int AreaLevelId { get; set; }
        public string? AreaLevelName { get; set; }
        public int SeniorityLevelId { get; set; }
        public string? SeniorityLevelName { get; set; }
        public string ProfileName { get; set; } = null!;
        public string ProfileDescription { get; set; } = null!;
        public int SortOrder { get; set; }
        public bool IsEnabled { get; set; } = true;



        //Audith
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}