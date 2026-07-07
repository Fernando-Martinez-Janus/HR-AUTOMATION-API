public class VacancyInputModel
{
    public int? OrganizationId { get; set; }
    public int? ProfileId { get; set; }
    public int? CriticalityLevelId { get; set; }
    public int? VacancyStatusId { get; set; }
    public string VacancyTitle { get; set; } = string.Empty;
    public string? ClientName { get; set; }
    public string? ProjectName { get; set; }
    public string? VacancyLocation { get; set; }
    public int PositionCount { get; set; } = 1;
    public decimal? SalaryRangeMin { get; set; }
    public decimal? SalaryRangeMax { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public int CreatedBy { get; set; }

    public void Normalize()
    {
        VacancyTitle = VacancyTitle.Trim();
        ClientName = ClientName?.Trim();
        ProjectName = ProjectName?.Trim();
        VacancyLocation = VacancyLocation?.Trim();
    }
}
