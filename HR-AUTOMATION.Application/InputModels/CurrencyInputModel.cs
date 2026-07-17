namespace HR_AUTOMATION.Application.InputModels
{
    public class CurrencyInputModel
    {
        public int? OrganizationId { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public string CurrencyName { get; set; } = null!;
        public string? CurrencySymbol { get; set; }

        public void Normalize()
        {
            CurrencyCode = CurrencyCode.Trim();
            CurrencyName = CurrencyName.Trim();
            CurrencySymbol = string.IsNullOrWhiteSpace(CurrencySymbol) ? null : CurrencySymbol.Trim();
        }
    }
}
