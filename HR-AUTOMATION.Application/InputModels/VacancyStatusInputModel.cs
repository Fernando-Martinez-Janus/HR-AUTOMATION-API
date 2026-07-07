public class VacancyStatusInputModel
{
    public int OrganizationId { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string StatusDescription { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public int CreatedBy { get; set; }

    public void Normalize()
    {
        StatusName = StatusName.Trim();
        StatusDescription = StatusDescription.Trim();
    }
}
