public class SeniorityLevelViewModel
{
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public string SeniorityName { get; set; } = string.Empty;
    public string? OrganizationName { get; set; }
    public int? SortOrder { get; set; }
    public bool IsEnabled { get; set; }
    public int TotalRecords { get; set; }
}
