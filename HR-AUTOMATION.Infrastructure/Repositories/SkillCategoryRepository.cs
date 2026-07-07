using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Shared.Kernel.IRepositories;

namespace HR_AUTOMATION.Infrastructure.Repositories
{
    public class SkillCategoryRepository : ISkillCategoryRepository
    {
        private readonly ISharedRepository _shared;

        public SkillCategoryRepository(ISharedRepository shared)
        {
            _shared = shared;
        }

        public async Task<IEnumerable<SkillCategory>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@page_number", page_number),
                new("@rows_page", rows_page)
            ];
            return await _shared.QueryAsync<SkillCategory>("config.web_get_skill_categories", parameters);
        }

        public async Task<SkillCategory?> GetByIdAsync(int id)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_skill_category_id", id)
            ];
            var result = await _shared.QueryAsync<SkillCategory>("config.web_get_skill_category_by_id", parameters);
            return result.FirstOrDefault();
        }

        public async Task<int> CreateAsync(int organizationId, string categoryName, int createdBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@p_category_name", categoryName),
                new("@p_created_by", createdBy)
            ];
            object? result = await _shared.QueryScalarAsync("config.web_ins_skill_category", parameters);
            return Convert.ToInt32(result);
        }

        public async Task UpdateAsync(int id, string categoryName, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_skill_category_id", id),
                new("@p_category_name", categoryName),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("config.web_upd_skill_category", parameters);
        }

        public async Task SoftDeleteAsync(int id, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_skill_category_id", id),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("config.web_del_skill_category", parameters);
        }
    }
}
