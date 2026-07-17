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
    }
}
