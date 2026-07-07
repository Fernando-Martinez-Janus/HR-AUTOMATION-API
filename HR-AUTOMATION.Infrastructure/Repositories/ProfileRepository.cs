using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Shared.Kernel.IRepositories;

namespace HR_AUTOMATION.Infrastructure.Repositories
{
    /// <summary>Provides data access for Profile entities using Entity Framework Core.</summary>
    public class ProfileRepository : IProfileRepository
    {
        private readonly ISharedRepository _shared;

        public ProfileRepository(ISharedRepository shared)
        {
            _shared = shared;
        }



        /// <summary>Gets a user by ID, including roles.</summary>
        public async Task<Profile?> GetByIdAsync(int profileId, int organizationId)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_profile_id", profileId),
                new("@p_organization_id", organizationId),
            ];

            var result = await _shared.QueryAsync<Profile>("recruitment.web_get_recruitment_profiles_by_id", parameters);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Profile>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@page_number", page_number),
                new("@rows_page", rows_page)
            ];

            return await _shared.QueryAsync<Profile>("recruitment.web_get_recruitment_profiles", parameters);
        }

        public async Task CreateAsync(int organizationId, int areaLevelId, int seniorityLevelId, string profileName, string profileDescription, int sortOrder, int createdBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@p_area_level_id", areaLevelId),
                new("@p_seniority_level_id", seniorityLevelId),
                new("@p_profile_name", profileName),
                new("@p_profile_description", profileDescription),
                new("@p_sort_order", sortOrder),
                new("@p_created_by", createdBy)
            ];

            await _shared.ExecuteAsync("recruitment.web_ins_recruitment_profiles", parameters);
        }


        public async Task UpdateAsync(int profileId, int areaLevelId, int seniorityLevelId, int organizationId, string profileName, string profileDescription, int sortOrder, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_profile_id", profileId),
                new("@p_area_level_id", areaLevelId),
                new("@p_seniority_level_id", seniorityLevelId),
                new("@p_organization_id", organizationId),
                new("@p_profile_name", profileName),
                new("@p_profile_description", profileDescription),
                new("@p_sort_order", sortOrder),
                new("@p_updated_by", updatedBy)

            ];

            await _shared.ExecuteAsync("recruitment.web_upd_recruitment_profiles", parameters);
        }

        public async Task SoftDeleteAsync(int profileId, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_profile_id", profileId),
                new("@p_updated_by", updatedBy)
            ];

            await _shared.ExecuteAsync("recruitment.web_del_recruitment_profile", parameters);
        }





    }
}
