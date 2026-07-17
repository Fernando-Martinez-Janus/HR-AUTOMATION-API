namespace HR_AUTOMATION.Application.ViewModels
{
    public class VacancyViewModel
    {
        public int Id { get; set; }
        public int? OrganizationId { get; set; }
        public int? ProfileId { get; set; }
        public string? ProfileName { get; set; }
        public int? CriticalityLevelId { get; set; }
        public int? VacancyStatusId { get; set; }
        public string? StatusName { get; set; }
        public string VacancyTitle { get; set; } = string.Empty;
        public string? ClientName { get; set; }
        public string? ProjectName { get; set; }
        public string? VacancyLocation { get; set; }
        public int PositionCount { get; set; }
        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public int? ModalityId { get; set; }
        public int? ContractTypeId { get; set; }
        public int? CurrencyId { get; set; }
        public int? PayFrequencyId { get; set; }
        public string? Notes { get; set; }
        public bool IsEnabled { get; set; }
    }
}
