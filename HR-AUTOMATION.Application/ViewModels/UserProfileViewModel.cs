namespace HR_AUTOMATION.Application.ViewModels
{
    /// <summary>
    /// Represents a user's profile information: identity, role, organization, and permissions.
    /// </summary>
    public class UserProfileViewModel
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user's full name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the user's role.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user's role.
        /// </summary>
        public string RoleName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the user's organization.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user's organization.
        /// </summary>
        public string OrganizationName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user's permissions, keyed by permission name.
        /// </summary>
        public Dictionary<string, bool> Permissions { get; set; } = [];
    }
}
