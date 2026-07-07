using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Shared.Kernel.IRepositories;

namespace HR_AUTOMATION.Infrastructure.Repositories
{
    public class AreaLevelRepository : IAreaLevelRepository
    {
        private readonly ISharedRepository _shared;

        public AreaLevelRepository(ISharedRepository shared)
        {
            _shared = shared;
        }

        public async Task<IEnumerable<AreaLevel>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@page_number", page_number),
                new("@rows_page", rows_page)
            ];

            return await _shared.QueryAsync<AreaLevel>("config.web_get_area_levels", parameters);
        }

        public async Task<AreaLevel?> GetByIdAsync(int areaLevelId)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_area_level_id", areaLevelId)
            ];

            var result = await _shared.QueryAsync<AreaLevel>("config.web_get_area_level_by_id", parameters);
            return result.FirstOrDefault();
        }

        public async Task CreateAsync(int organizationId, string areaLevelName, string areaLevelDescription, int sortOrder, int createdBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@p_level_name", areaLevelName),
                new("@p_level_description", areaLevelDescription),
                new("@p_sort_order", sortOrder),
                new("@p_created_by", createdBy)
            ];

            await _shared.ExecuteAsync("config.web_ins_area_level", parameters);
        }

        public async Task UpdateAsync(int areaLevelId, int organizationId, string areaLevelName, string areaLevelDescription, int sortOrder, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_area_level_id", areaLevelId),
                new("@p_organization_id", organizationId),
                new("@p_level_name", areaLevelName),
                new("@p_level_description", areaLevelDescription),
                new("@p_sort_order", sortOrder),
                new("@p_updated_by", updatedBy)
            ];

            await _shared.ExecuteAsync("config.web_upd_area_level", parameters);
        }

        public async Task SoftDeleteAsync(int areaLevelId, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_area_level_id", areaLevelId),
                new("@p_updated_by", updatedBy)
            ];

            await _shared.ExecuteAsync("config.web_del_area_level", parameters);
        }
    }
}
