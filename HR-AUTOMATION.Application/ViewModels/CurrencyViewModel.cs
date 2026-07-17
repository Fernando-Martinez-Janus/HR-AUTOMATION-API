namespace HR_AUTOMATION.Application.ViewModels
{
    public class CurrencyViewModel
    {
        public int CurrencyId { get; set; }
        public int OrganizationId { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public string CurrencyName { get; set; } = null!;
        public string? CurrencySymbol { get; set; }
        public int SortOrder { get; set; }
    }
}
