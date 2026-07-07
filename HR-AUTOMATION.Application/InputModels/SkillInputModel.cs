public class SkillInputModel
{
    public int OrganizationId { get; set; }
    public int SkillCategoryId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public int CreatedBy { get; set; }

    public void Normalize()
    {
        SkillName = SkillName.Trim();
    }
}
