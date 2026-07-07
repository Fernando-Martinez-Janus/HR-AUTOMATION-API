using HR_AUTOMATION.Domain.Models;

namespace HR_AUTOMATION.Domain.IRepositories
{
    public interface ISkillCategoryRepository
    {
        Task<IEnumerable<SkillCategory>> GetAllAsync(int organizationId, int rows_page, int page_number);
        Task<SkillCategory?> GetByIdAsync(int id);
        Task<int> CreateAsync(int organizationId, string categoryName, int createdBy);
        Task UpdateAsync(int id, string categoryName, int updatedBy);
        Task SoftDeleteAsync(int id, int updatedBy);
    }
}
