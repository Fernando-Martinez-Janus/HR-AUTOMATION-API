using HR_AUTOMATION.Domain.Models;

namespace HR_AUTOMATION.Domain.IRepositories
{
    public interface IVacancyStatusRepository
    {
        Task<IEnumerable<VacancyStatus>> GetAllAsync(int organizationId, int rows_page, int page_number);
        Task<VacancyStatus?> GetByIdAsync(int id);
        Task<int> CreateAsync(int organizationId, string statusName, string statusDescription, int sortOrder, int createdBy);
        Task UpdateAsync(int id, string statusName, string statusDescription, int sortOrder, int updatedBy);
        Task SoftDeleteAsync(int id, int updatedBy);
    }
}
