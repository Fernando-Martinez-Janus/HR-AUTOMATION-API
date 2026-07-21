using Shared.Kernel.InputModels;

namespace HR_AUTOMATION.Application.InputModels
{
    /// <summary>
    /// Represents the input model used to search and filter vacancies.
    /// </summary>
    public class VacancySearchInputModel : PaginationRequest
    {
        /// <summary>
        /// Gets or sets the organization identifier used to filter the results.
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the status name to filter by.
        /// </summary>
        public string? StatusName { get; set; }

        /// <summary>
        /// Gets or sets the vacancy location to filter by.
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Gets or sets the criticality level identifier to filter by.
        /// </summary>
        public int? CriticalityLevelId { get; set; }
    }
}
