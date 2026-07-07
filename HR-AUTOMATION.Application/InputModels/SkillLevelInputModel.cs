namespace HR_AUTOMATION.Application.InputModels
{
    public class SkillLevelInputModel
    {
        public int OrganizationId { get; set; }
        public string SkillLevelName { get; set; } = string.Empty;
        public string SkillLevelDescription { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public int CreatedBy { get; set; }


        // Pagination
        public int? RowsPage { get; set; }
        public int? PageNumber { get; set; }



        public void Normalize()
        {
            SkillLevelName = SkillLevelName.Trim();
        }
    }
}
