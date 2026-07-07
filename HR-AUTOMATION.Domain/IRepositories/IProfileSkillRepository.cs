using HR_AUTOMATION.Domain.Models;

namespace HR_AUTOMATION.Domain.IRepositories
{
    public interface IProfileSkillRepository
    {
        Task<IEnumerable<ProfileSkill>> GetAllAsync(int organizationId, int rows_page, int page_number);
        Task<ProfileSkill?> GetByIdAsync(int id);
        Task<int> CreateAsync(int profileId, int skillId, int skillLevelId, string skillWeight, int createdBy);
        Task UpdateAsync(int id, int skillId, int skillLevelId, string skillWeight, int updatedBy);
        Task SoftDeleteAsync(int id, int updatedBy);
    }
}
