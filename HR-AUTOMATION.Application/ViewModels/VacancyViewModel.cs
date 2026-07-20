namespace HR_AUTOMATION.Application.ViewModels
{
    /// <summary>
    /// Represents a vacancy as returned by the API.
    /// </summary>
    public class VacancyViewModel
    {
        /// <summary>
        /// Gets or sets the vacancy identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the profile identifier.
        /// </summary>
        public int? ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the profile name.
        /// </summary>
        public string? ProfileName { get; set; }

        /// <summary>
        /// Gets or sets the criticality level identifier.
        /// </summary>
        public int? CriticalityLevelId { get; set; }

        /// <summary>
        /// Gets or sets the vacancy status identifier.
        /// </summary>
        public int? VacancyStatusId { get; set; }

        /// <summary>
        /// Gets or sets the status name.
        /// </summary>
        public string? StatusName { get; set; }

        /// <summary>
        /// Gets or sets the vacancy title.
        /// </summary>
        public string VacancyTitle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the client name.
        /// </summary>
        public string? ClientName { get; set; }

        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string? ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the vacancy location.
        /// </summary>
        public string? VacancyLocation { get; set; }

        /// <summary>
        /// Gets or sets the number of open positions.
        /// </summary>
        public int PositionCount { get; set; }

        /// <summary>
        /// Gets or sets the minimum salary range.
        /// </summary>
        public decimal? SalaryRangeMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum salary range.
        /// </summary>
        public decimal? SalaryRangeMax { get; set; }

        /// <summary>
        /// Gets or sets the request date.
        /// </summary>
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the deadline date.
        /// </summary>
        public DateTime? DeadlineDate { get; set; }

        /// <summary>
        /// Gets or sets the work modality identifier.
        /// </summary>
        public int? WorkModalityId { get; set; }

        /// <summary>
        /// Gets or sets the employment type identifier.
        /// </summary>
        public int? EmploymentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the currency identifier.
        /// </summary>
        public int? CurrencyId { get; set; }

        /// <summary>
        /// Gets or sets the payment period identifier.
        /// </summary>
        public int? PaymentPeriodId { get; set; }

        /// <summary>
        /// Gets or sets additional notes.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets whether the vacancy is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
