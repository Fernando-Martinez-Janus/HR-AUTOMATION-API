namespace HR_AUTOMATION.Domain.Models
{
    /// <summary>
    /// Represents the domain model for a profile entity.
    /// </summary>
    public class ProfileModel
    {
        /// <summary>
        /// Gets or sets the profile unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the area level identifier.
        /// </summary>
        public int AreaLevelId { get; set; }

        /// <summary>
        /// Gets or sets the area level name (if retrieved via join).
        /// </summary>
        public string? AreaLevelName { get; set; }

        /// <summary>
        /// Gets or sets the seniority level identifier.
        /// </summary>
        public int SeniorityLevelId { get; set; }

        /// <summary>
        /// Gets or sets the seniority level name (if retrieved via join).
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
        /// Gets or sets the JSON string or formatted list of associated skills.
        /// </summary>
        public string? Skills { get; set; }

        /// <summary>
        /// Gets or sets the total count of records (used for pagination in search stored procedures).
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the creation timestamp.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user identifier who created the record.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last update timestamp.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user identifier who last updated the record.
        /// </summary>
        public int? UpdatedBy { get; set; }
    }
}