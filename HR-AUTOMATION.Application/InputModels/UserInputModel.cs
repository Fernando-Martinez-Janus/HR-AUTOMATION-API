public class UserInputModel
{
    public int OrganizationId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; }
    public List<long> RolesIds { get; set; } = new();
    public int CreatedBy { get; set; }

    public void Normalize()
    {
        Username = Username.Trim();
        Email = Email.Trim();
    }
}
