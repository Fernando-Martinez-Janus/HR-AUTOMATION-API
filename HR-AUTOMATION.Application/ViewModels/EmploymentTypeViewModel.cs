namespace HR_AUTOMATION.Application.ViewModels
{
    public class EmploymentTypeViewModel
    {
        public int EmploymentTypeId { get; set; }
        public int OrganizationId { get; set; }
        public string TypeName { get; set; } = null!;
        public int SortOrder { get; set; }
    }
}
