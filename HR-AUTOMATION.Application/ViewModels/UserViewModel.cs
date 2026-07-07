public class UserViewModel
{
    public long Id { get; set; }
    public int OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public int TotalRecords { get; set; }
}
