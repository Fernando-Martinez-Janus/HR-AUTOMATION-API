using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Entities
{
    public class User
    {
        [Column("user_id")]
        public int Id { get; set; }

        [Column("username")]
        public string FullName { get; set; } = null!;

        [Column("email")]
        public string Email { get; set; } = null!;

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("role_name")]
        public string RoleName { get; set; } = null!;

        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Column("organization_name")]
        public string OrganizationName { get; set; } = null!;
    }
}
