namespace HR_AUTOMATION.Application.InputModels
{
    public class AreaLevelInputModel
    {
        public int OrganizationId { get; set; }
        public string AreaLevelName { get; set; } = string.Empty;
        public string AreaLevelDescription { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }


        // Pagination
        public int? RowsPage { get; set; }
        public int? PageNumber { get; set; }



        public void Normalize()
        {
            AreaLevelName = AreaLevelName.Trim();
        }
    }
}
