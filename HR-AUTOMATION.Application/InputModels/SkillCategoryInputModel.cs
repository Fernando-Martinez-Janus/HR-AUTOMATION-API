public class SkillCategoryInputModel
{
    public int OrganizationId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int CreatedBy { get; set; }

    public void Normalize()
    {
        CategoryName = CategoryName.Trim();
    }
}
