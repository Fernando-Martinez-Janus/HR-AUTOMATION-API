using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Shared.Kernel.IRepositories;

namespace HR_AUTOMATION.Infrastructure.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly ISharedRepository _shared;

        public SkillRepository(ISharedRepository shared)
        {
            _shared = shared;
        }

        public async Task<IEnumerable<Skill>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@page_number", page_number),
                new("@rows_page", rows_page)
            ];
            return await _shared.QueryAsync<Skill>("config.web_get_skills", parameters);
        }

        public async Task<Skill?> GetByIdAsync(int id)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_skill_id", id)
            ];
            var result = await _shared.QueryAsync<Skill>("config.web_get_skill_by_id", parameters);
            return result.FirstOrDefault();
        }

        public async Task<int> CreateAsync(int organizationId, int skillCategoryId, string skillName, int createdBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@p_skill_category_id", skillCategoryId),
                new("@p_skill_name", skillName),
                new("@p_created_by", createdBy)
            ];
            object? result = await _shared.QueryScalarAsync("config.web_ins_skill", parameters);
            return Convert.ToInt32(result);
        }

        public async Task UpdateAsync(int id, int skillCategoryId, string skillName, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_skill_id", id),
                new("@p_skill_category_id", skillCategoryId),
                new("@p_skill_name", skillName),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("config.web_upd_skill", parameters);
        }

        public async Task SoftDeleteAsync(int id, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_skill_id", id),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("config.web_del_skill", parameters);
        }
    }
}
