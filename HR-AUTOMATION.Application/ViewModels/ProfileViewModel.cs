
public class ProfileViewModel
{
    public int ProfileId { get; set; }
    public int OrganizationId { get; set; }
    public int AreaLevelId { get; set; }
    public string? AreaLevelName { get; set; }
    public int? SeniorityLevelId { get; set; }
    public string? SeniorityLevelName { get; set; }
    public string ProfileName { get; set; } = null!;
    public string ProfileDescription { get; set; } = null!;
    public int SortOrder { get; set; }
    public bool IsEnabled { get; set; } = true;


    //public List<RoleSimpleViewModel>? Roles { get; set; }
}


//public class PrifileSkillSimpleViewModel
//{
//    public Guid Id { get; set; }
//    public required string RoleName { get; set; }
//}
