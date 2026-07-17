namespace HR_AUTOMATION.Application.InputModels
{
    public class PaymentPeriodInputModel
    {
        public int? OrganizationId { get; set; }
        public string PeriodName { get; set; } = null!;

        public void Normalize()
        {
            PeriodName = PeriodName.Trim();
        }
    }
}
