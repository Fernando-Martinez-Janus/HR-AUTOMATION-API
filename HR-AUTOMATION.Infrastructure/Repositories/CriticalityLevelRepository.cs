using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Shared.Kernel.IRepositories;

namespace HR_AUTOMATION.Infrastructure.Repositories
{
    public class CriticalityLevelRepository : ICriticalityLevelRepository
    {
        private readonly ISharedRepository _shared;

        public CriticalityLevelRepository(ISharedRepository shared)
        {
            _shared = shared;
        }

        public async Task<IEnumerable<CriticalityLevel>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@page_number", page_number),
                new("@rows_page", rows_page)
            ];

            return await _shared.QueryAsync<CriticalityLevel>("config.web_get_criticality_levels", parameters);
        }

        public async Task<CriticalityLevel?> GetByIdAsync(int criticalityLevelId)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_criticality_level_id", criticalityLevelId)
            ];

            var result = await _shared.QueryAsync<CriticalityLevel>("config.web_get_criticality_level_by_id", parameters);
            return result.FirstOrDefault();
        }

        public async Task CreateAsync(int organizationId, string criticalityLevelName, string criticalityLevelDescription, int sortOrder, int createdBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@p_criticality_level_name", criticalityLevelName),
                new("@p_criticality_level_description", criticalityLevelDescription),
                new("@p_sort_order", sortOrder),
                new("@p_created_by", createdBy)
            ];

            await _shared.ExecuteAsync("config.web_ins_criticality_level", parameters);
        }

        public async Task UpdateAsync(int criticalityLevelId, int organizationId, string criticalityLevelName, string criticalityLevelDescription, int sortOrder, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_criticality_level_id", criticalityLevelId),
                new("@p_organization_id", organizationId),
                new("@p_level_name", criticalityLevelName),
                new("@p_level_description", criticalityLevelDescription),
                new("@p_sort_order", sortOrder),
                new("@p_updated_by", updatedBy)
            ];

            await _shared.ExecuteAsync("config.web_upd_criticality_level", parameters);
        }

        public async Task SoftDeleteAsync(int criticalityLevelId, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_criticality_level_id", criticalityLevelId),
                new("@p_updated_by", updatedBy)
            ];

            await _shared.ExecuteAsync("config.web_del_criticality_level", parameters);
        }
    }
}
