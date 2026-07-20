using Shared.Kernel.Utils.Constants;

namespace HR_AUTOMATION.Application.InputModels
{
    /// <summary>
    /// Represents the input model used to create, update, or upsert a vacancy.
    /// If <see cref="VacancyId"/> is <see langword="null"/> a new vacancy is created;
    /// otherwise the existing vacancy is updated.
    /// </summary>
    public class VacancyInputModel
    {
        /// <summary>
        /// Gets or sets the vacancy identifier. Used by upsert to determine whether to insert or update.
        /// </summary>
        public int? VacancyId { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the profile identifier.
        /// </summary>
        public int? ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the criticality level identifier.
        /// </summary>
        public int? CriticalityLevelId { get; set; }

        /// <summary>
        /// Gets or sets the vacancy status identifier.
        /// </summary>
        public int? VacancyStatusId { get; set; }

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
        public int PositionCount { get; set; } = 1;

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
        /// Cleans and normalizes the input values by trimming string properties.
        /// </summary>
        public void Normalize()
        {
            VacancyTitle = VacancyTitle.Trim();
            ClientName = ClientName?.Trim();
            ProjectName = ProjectName?.Trim();
            VacancyLocation = VacancyLocation?.Trim();
            Notes = Notes?.Trim();
        }
    }
}
