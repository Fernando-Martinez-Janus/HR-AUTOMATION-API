public class RoleViewModel
{
    public long Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public List<PermissionSimpleViewModel>? Permissions { get; set; }
    public int TotalRecords { get; set; }
}

public class PermissionSimpleViewModel
{
    public long Id { get; set; }
    public string PermissionName { get; set; } = string.Empty;
}
