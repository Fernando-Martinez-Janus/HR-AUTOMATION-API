namespace HR_AUTOMATION.Application.ViewModels
{
    /// <summary>
    /// Represents the view data returned for a question category.
    /// </summary>
    public class QuestionCategoryViewModel
    {
        /// <summary>
        /// Gets or sets the question category identifier.
        /// </summary>
        public int QuestionCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the question category.
        /// </summary>
        public string CategoryName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the question category.
        /// </summary>
        public string CategoryDescription { get; set; } = null!;
    }
}
