namespace HR_AUTOMATION.Application.ViewModels
{
    public class WorkModalityViewModel
    {
        public int WorkModalityId { get; set; }
        public int OrganizationId { get; set; }
        public string ModalityName { get; set; } = null!;
        public int SortOrder { get; set; }
    }
}
