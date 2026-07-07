using HR_AUTOMATION.Domain.Models;

namespace HR_AUTOMATION.Domain.IRepositories
{
    public interface ISkillLevelRepository
    {

        /// <summary>Gets all Skill levels for an organization.</summary>
        Task<IEnumerable<SkillLevel>> GetAllAsync(int organizationId, int rows_page, int page_number);

        /// <summary>Gets a Skill level by its identifier.</summary>
        Task<SkillLevel?> GetByIdAsync(int skillLevelId);

        /// <summary>Creates a new Skill level.</summary>
        Task CreateAsync(int organizationId, string skillLevelName, string skillLevelDescription, int sortOrder, int createdBy);

        /// <summary>Updates an existing Skill level.</summary>
        Task UpdateAsync(int id, int organizationId, string skillLevelName, string skillLevelDescription, int sortOrder, int updatedBy);

        /// <summary>Soft-deletes (disables) a Skill level.</summary>
        Task SoftDeleteAsync(int skillLevelId, int updatedBy);
    }
}
