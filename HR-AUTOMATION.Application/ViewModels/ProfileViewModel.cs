namespace HR_AUTOMATION.Application.ViewModels
{
    /// <summary>
    /// Represents the view model for displaying profile information.
    /// </summary>
    public class ProfileViewModel
    {
        /// <summary>
        /// Gets or sets the profile identifier.
        /// </summary>
        public int ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the area level identifier.
        /// </summary>
        public int AreaLevelId { get; set; }

        /// <summary>
        /// Gets or sets the area level name.
        /// </summary>
        public string? AreaLevelName { get; set; }

        /// <summary>
        /// Gets or sets the seniority level identifier.
        /// </summary>
        public int SeniorityLevelId { get; set; }

        /// <summary>
        /// Gets or sets the seniority level name.
        /// </summary>
        public string? SeniorityLevelName { get; set; }

        /// <summary>
        /// Gets or sets the profile name.
        /// </summary>
        public string ProfileName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the profile description.
        /// </summary>
        public string? ProfileDescription { get; set; }

        /// <summary>
        /// Gets or sets the collection of skills associated with the profile.
        /// </summary>
        public IEnumerable<ProfileSkillViewModel> Skills { get; set; } = [];
    }
}