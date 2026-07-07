using HR_AUTOMATION.Domain.Models;

namespace HR_AUTOMATION.Domain.IRepositories
{
    public interface IVacancyRepository
    {
        Task<IEnumerable<Vacancy>> GetAllAsync(int organizationId, int rows_page, int page_number);
        Task<Vacancy?> GetByIdAsync(int id);
        Task<int> CreateAsync(int? organizationId, int? profileId, int? criticalityLevelId, int? vacancyStatusId,
            string vacancyTitle, string? clientName, string? projectName, string? vacancyLocation,
            int positionCount, decimal? salaryRangeMin, decimal? salaryRangeMax,
            DateTime? requestDate, DateTime? deadlineDate, int createdBy);
        Task UpdateAsync(int id, int? profileId, int? criticalityLevelId, int? vacancyStatusId,
            string vacancyTitle, string? clientName, string? projectName, string? vacancyLocation,
            int positionCount, decimal? salaryRangeMin, decimal? salaryRangeMax,
            DateTime? requestDate, DateTime? deadlineDate, int updatedBy);
        Task SoftDeleteAsync(int id, int updatedBy);
    }
}
