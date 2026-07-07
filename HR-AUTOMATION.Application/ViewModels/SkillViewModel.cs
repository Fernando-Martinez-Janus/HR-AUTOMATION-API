public class SkillViewModel
{
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public int SkillCategoryId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public int TotalRecords { get; set; }
}
