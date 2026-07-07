public class ProfileSkillViewModel
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public int SkillId { get; set; }
    public int SkillLevelId { get; set; }
    public string SkillWeight { get; set; } = string.Empty;
    public string SkillName { get; set; } = string.Empty;
    public string LevelName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public int TotalRecords { get; set; }
}
