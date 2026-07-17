using Shared.Kernel.InputModels;

namespace HR_AUTOMATION.Application.InputModels
{
    /// <summary>
    /// Represents the search criteria used to filter question categories.
    /// </summary>
    public class QuestionCategorySearchInputModel : PaginationRequest
    {
        /// <summary>
        /// Gets or sets the organization identifier to filter by.
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Normalizes the search input by applying base normalization.
        /// </summary>
        public new void Normalize()
        {
            base.Normalize();
        }
    }
}
