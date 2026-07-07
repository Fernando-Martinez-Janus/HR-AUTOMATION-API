public class ProfileSkillInputModel
{
    public int ProfileId { get; set; }
    public int OrganizationId { get; set; }
    public int SkillId { get; set; }
    public int SkillLevelId { get; set; }
    public string SkillWeight { get; set; } = string.Empty;
    public int CreatedBy { get; set; }

    public void Normalize()
    {
        SkillWeight = SkillWeight.Trim().ToLower();
    }
}
