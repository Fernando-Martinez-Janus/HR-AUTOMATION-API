using Shared.Kernel.InputModels;

namespace HR_AUTOMATION.Application.InputModels
{
    /// <summary>
    /// Represents the input model used to search and filter skills.
    /// </summary>
    public class SkillSearchInputModel : PaginationRequest
    {
        /// <summary>
        /// Gets or sets the organization identifier used to filter the results.
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the skill categories used to filter the results.
        /// </summary>
        public List<int> SkillCategories { get; set; } = [];

        /// <summary>
        /// Normalizes the filter values.
        /// </summary>
        /// <remarks>
        /// Calls the base <see cref="PaginationRequest.Normalize"/> method.
        /// Override this method to add custom normalization logic for user filters.
        /// </remarks>
        public override void Normalize()
        {
            SkillCategories = [.. SkillCategories.Distinct()];

            base.Normalize();
        }
    }
}