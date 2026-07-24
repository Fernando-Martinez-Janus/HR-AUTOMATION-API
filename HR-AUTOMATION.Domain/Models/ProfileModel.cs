using HR_AUTOMATION.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Models
{
    /// <summary>
    /// Represents the domain model for a profile entity.
    /// </summary>
    public class ProfileModel : Profile
    {
        /// <summary>
        /// Gets or sets the area level name (if retrieved via join).
        /// </summary>
        [Column("profile_name")]
        public string? AreaLevelName { get; set; }

        /// <summary>
        /// Gets or sets the seniority level name (if retrieved via join).
        /// </summary>
        [Column("seniority_level_name")]
        public string? SeniorityLevelName { get; set; }

        /// <summary>
        /// Gets or sets the JSON string or formatted list of associated skills.
        /// </summary>
        [Column("skills")]
        public IEnumerable<ProfileSkillModel> Skills { get; set; } = [];

        /// <summary>
        /// Gets or sets the total count of records (used for pagination in search stored procedures).
        /// </summary>
        [Column("total_count")]
        public int TotalCount { get; set; }
    }
}