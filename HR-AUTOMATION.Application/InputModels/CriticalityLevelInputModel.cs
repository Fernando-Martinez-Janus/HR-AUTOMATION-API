namespace HR_AUTOMATION.Application.InputModels
{
    public class CriticalityLevelInputModel
    {
        public int OrganizationId { get; set; }
        public string CriticalityLevelName { get; set; } = string.Empty;
        public string CriticalityLevelDescription { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public int CreatedBy { get; set; }


        // Pagination
        public int? RowsPage { get; set; }
        public int? PageNumber { get; set; }



        public void Normalize()
        {
            CriticalityLevelName = CriticalityLevelName.Trim();
        }
    }
}