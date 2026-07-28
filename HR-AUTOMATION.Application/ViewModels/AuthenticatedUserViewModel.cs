namespace HR_AUTOMATION.Application.ViewModels
{
    /// <summary>
    /// Represents the authenticated user information returned after a successful login.
    /// </summary>
    public class AuthenticatedUserViewModel
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
        public string Role { get; set; } = null!;

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
