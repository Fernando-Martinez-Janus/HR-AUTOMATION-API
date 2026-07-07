public class PermissionInputModel
{
    public int OrganizationId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public int CreatedBy { get; set; }

    public void Normalize()
    {
        PermissionName = PermissionName.Trim();
    }
}
