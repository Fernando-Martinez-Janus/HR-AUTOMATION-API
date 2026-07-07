using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Shared.Kernel.IRepositories;

namespace HR_AUTOMATION.Infrastructure.Repositories
{
    public class ProfileSkillRepository : IProfileSkillRepository
    {
        private readonly ISharedRepository _shared;

        public ProfileSkillRepository(ISharedRepository shared)
        {
            _shared = shared;
        }

        public async Task<IEnumerable<ProfileSkill>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@page_number", page_number),
                new("@rows_page", rows_page)
            ];
            return await _shared.QueryAsync<ProfileSkill>("recruitment.web_get_profile_skills", parameters);
        }

        public async Task<ProfileSkill?> GetByIdAsync(int id)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_profile_skill_id", id)
            ];
            var result = await _shared.QueryAsync<ProfileSkill>("recruitment.web_get_profile_skill_by_id", parameters);
            return result.FirstOrDefault();
        }

        public async Task<int> CreateAsync(int profileId, int skillId, int skillLevelId, string skillWeight, int createdBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_profile_id", profileId),
                new("@p_skill_id", skillId),
                new("@p_skill_level_id", skillLevelId),
                new("@p_skill_weight", skillWeight),
                new("@p_created_by", createdBy)
            ];
            object? result = await _shared.QueryScalarAsync("recruitment.web_ins_profile_skill", parameters);
            return Convert.ToInt32(result);
        }

        public async Task UpdateAsync(int id, int skillId, int skillLevelId, string skillWeight, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_profile_skill_id", id),
                new("@p_skill_id", skillId),
                new("@p_skill_level_id", skillLevelId),
                new("@p_skill_weight", skillWeight),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("recruitment.web_upd_profile_skill", parameters);
        }

        public async Task SoftDeleteAsync(int id, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_profile_skill_id", id),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("recruitment.web_del_profile_skill", parameters);
        }
    }
}
