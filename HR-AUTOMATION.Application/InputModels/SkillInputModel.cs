namespace HR_AUTOMATION.Application.InputModels
{
    public class SkillInputModel
    {
        public int? OrganizationId { get; set; }
        public int SkillCategoryId { get; set; }
        public string SkillName { get; set; } = null!;

        public void Normalize()
        {
            SkillName = SkillName.Trim();
        }
    }
}
