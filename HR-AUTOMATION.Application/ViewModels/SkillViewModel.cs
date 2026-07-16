namespace HR_AUTOMATION.Application.ViewModels
{
    public class SkillViewModel
    {
        public int SkillId { get; set; }
        public int OrganizationId { get; set; }
        public int SkillCategoryId { get; set; }
        public string SkillName { get; set; } = null!;
    }
}