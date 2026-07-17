namespace HR_AUTOMATION.Application.ViewModels
{
    /// <summary>
    /// Represents the view data returned for an organization.
    /// </summary>
    public class OrganizationViewModel
    {
        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the organization.
        /// </summary>
        public string OrganizationName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the URL-friendly slug of the organization.
        /// </summary>
        public string Slug { get; set; } = null!;
    }
}
