namespace HR_AUTOMATION.Application.InputModels
{
    /// <summary>
    /// Represents the input data required to create or update a question category.
    /// </summary>
    public class QuestionCategoryInputModel
    {
        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the question category.
        /// </summary>
        public string CategoryName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the question category.
        /// </summary>
        public string CategoryDescription { get; set; } = null!;

        /// <summary>
        /// Normalizes the input model by trimming whitespace from string properties.
        /// </summary>
        public void Normalize()
        {
            CategoryName = CategoryName?.Trim()!;
            CategoryDescription = CategoryDescription?.Trim()!;
        }
    }
}
