using HR_AUTOMATION.Domain.Models;

namespace HR_AUTOMATION.Domain.IRepositories
{
    public interface ISkillRepository
    {
        Task<IEnumerable<Skill>> GetAllAsync(int organizationId, int rows_page, int page_number);
        Task<Skill?> GetByIdAsync(int id);
        Task<int> CreateAsync(int organizationId, int skillCategoryId, string skillName, int createdBy);
        Task UpdateAsync(int id, int skillCategoryId, string skillName, int updatedBy);
        Task SoftDeleteAsync(int id, int updatedBy);
    }
}
