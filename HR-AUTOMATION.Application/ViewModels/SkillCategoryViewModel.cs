public class SkillCategoryViewModel
{
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public int TotalRecords { get; set; }
}
