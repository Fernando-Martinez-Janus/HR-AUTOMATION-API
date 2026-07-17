namespace HR_AUTOMATION.Application.ViewModels
{
    public class RejectionReasonViewModel
    {
        public int RejectionReasonId { get; set; }
        public int OrganizationId { get; set; }
        public string Description { get; set; } = null!;
        public bool IsDefinitive { get; set; }
    }
}
