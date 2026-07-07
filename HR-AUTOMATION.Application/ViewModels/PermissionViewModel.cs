public class PermissionViewModel
{
    public long Id { get; set; }
    public int OrganizationId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public int TotalRecords { get; set; }
}
