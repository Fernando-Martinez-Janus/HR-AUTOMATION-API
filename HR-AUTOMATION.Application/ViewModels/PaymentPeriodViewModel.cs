namespace HR_AUTOMATION.Application.ViewModels
{
    public class PaymentPeriodViewModel
    {
        public int PaymentPeriodId { get; set; }
        public int OrganizationId { get; set; }
        public string PeriodName { get; set; } = null!;
        public int SortOrder { get; set; }
    }
}
