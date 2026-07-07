public class VacancyStatusViewModel
{
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string StatusDescription { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsEnabled { get; set; }
    public int TotalRecords { get; set; }
}
