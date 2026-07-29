using HR_AUTOMATION.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Models
{
    /// <summary>
    /// Represents a single row returned by [auth].[web_get_user_by_email] or [auth].[web_login],
    /// both of which repeat the user's information once per permission. Not a domain concept on
    /// its own; used only to map the stored procedures' flattened result sets.
    /// </summary>
    public class UserPermissionRow : User
    {
        [Column("permission_id")]
        public int? PermissionId { get; set; }

        [Column("permission_name")]
        public string? PermissionName { get; set; }

        [Column("is_allowed")]
        public bool? IsAllowed { get; set; }

        /// <summary>
        /// The stored password hash, populated only by [auth].[web_login]. Null for Google Sign-In,
        /// since [auth].[web_get_user_by_email] does not return this column.
        /// </summary>
        [Column("password_hash")]
        public string? PasswordHash { get; set; }
    }
}
