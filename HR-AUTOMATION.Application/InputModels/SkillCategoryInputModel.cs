namespace HR_AUTOMATION.Application.InputModels
{
    public class SkillCategoryInputModel
    {
        public int? OrganizationId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? IconName { get; set; }

        public void Normalize()
        {
            CategoryName = CategoryName.Trim();
            IconName = string.IsNullOrWhiteSpace(IconName) ? null : IconName.Trim();
        }
    }
}