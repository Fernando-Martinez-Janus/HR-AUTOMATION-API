public class ProfileInputModel
{
    public int OrganizationId { get; set; }
    public int AreaLevelId { get; set; }
    public int SeniorityLevelId { get; set; }
    public string ProfileName { get; set; } = null!;
    public string ProfileDescription { get; set; } = null!;
    public int SortOrder { get; set; }
    public bool IsEnabled { get; set; } = true;
    public int CreatedBy { get; set; }

    //public List<Guid> RolesIds { get; set; } = new();

    public int UpdatedBy { get; set; }

    // Pagination
    public int? RowsPage { get; set; }
    public int? PageNumber { get; set; }



    public void Normalize()
    {
        ProfileName = ProfileName.Trim();
    }
}