using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Shared.Kernel.IRepositories;

namespace HR_AUTOMATION.Infrastructure.Repositories
{
    public class SkillLevelRepository : ISkillLevelRepository
    {
        private readonly ISharedRepository _shared;

        public SkillLevelRepository(ISharedRepository shared)
        {
            _shared = shared;
        }

        public async Task<IEnumerable<SkillLevel>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@page_number", page_number),
                new("@rows_page", rows_page)
            ];

            return await _shared.QueryAsync<SkillLevel>("config.web_get_skill_levels", parameters);
        }

        public async Task<SkillLevel?> GetByIdAsync(int skillLevelId)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_skill_level_id", skillLevelId)
            ];

            var result = await _shared.QueryAsync<SkillLevel>("config.web_get_skill_level_by_id", parameters);
            return result.FirstOrDefault();
        }

        public async Task CreateAsync(int organizationId, string skillLevelName, string skillLevelDescription, int sortOrder, int createdBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@p_skill_level_name", skillLevelName),
                new("@p_skill_level_description", skillLevelDescription),
                new("@p_sort_order", sortOrder),
                new("@p_created_by", createdBy)
            ];

            await _shared.ExecuteAsync("config.web_ins_skill_level", parameters);
        }

        public async Task UpdateAsync(int skillLevelId, int organizationId, string skillLevelName, string skillLevelDescription, int sortOrder, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_skill_level_id", skillLevelId),
                new("@p_organization_id", organizationId),
                new("@p_level_name", skillLevelName),
                new("@p_level_description", skillLevelDescription),
                new("@p_sort_order", sortOrder),
                new("@p_updated_by", updatedBy)
            ];

            await _shared.ExecuteAsync("config.web_upd_skill_level", parameters);
        }

        public async Task SoftDeleteAsync(int skillLevelId, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_skill_level_id", skillLevelId),
                new("@p_updated_by", updatedBy)
            ];

            await _shared.ExecuteAsync("config.web_del_skill_level", parameters);
        }
    }
}
