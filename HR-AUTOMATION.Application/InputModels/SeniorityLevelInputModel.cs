public class SeniorityLevelInputModel
{
    public int OrganizationId { get; set; }
    public string SeniorityName { get; set; } = string.Empty;
    public int? SortOrder { get; set; }
    public int CreatedBy { get; set; }

    public void Normalize()
    {
        SeniorityName = SeniorityName.Trim();
    }
}
