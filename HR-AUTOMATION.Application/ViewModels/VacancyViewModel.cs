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
        /// Gets or sets the minimum experience filter.
        /// </summary>
        public int? MinimumExperience { get; set; }

        /// <summary>
        /// Gets or sets the maximum experience filter.
        /// </summary>
        public int? MaximumExperience { get; set; }

        /// <summary>
        /// Gets or sets the scolarity identifier.
        /// </summary>
        public int? ScolarityId { get; set; }

        /// <summary>
        /// Gets or sets the skills profile JSON.
        /// </summary>
        public string? SkillsProfile { get; set; }

        /// <summary>
        /// Gets or sets the excluded keywords.
        /// </summary>
        public string? Excluded { get; set; }

        /// <summary>
        /// Gets or sets the included criteria.
        /// </summary>
        public string? Included { get; set; }

        /// <summary>
        /// Gets or sets the sources JSON array.
        /// </summary>
        public string? Sources { get; set; }

        /// <summary>
        /// Gets or sets the max CV age in days.
        /// </summary>
        public int? CvMaxAge { get; set; }

        /// <summary>
        /// Gets or sets the request cooldown in milliseconds.
        /// </summary>
        public int RequestCooldownMs { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of CVs.
        /// </summary>
        public int? MaxCvs { get; set; }

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
