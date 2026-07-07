public class RoleInputModel
{
    public int OrganizationId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public List<long> PermissionIds { get; set; } = [];
    public int CreatedBy { get; set; }

    public void Normalize()
    {
        RoleName = RoleName.Trim();
    }
}
