using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Shared.Kernel.IRepositories;

namespace HR_AUTOMATION.Infrastructure.Repositories
{
    /// <summary>Provides data access for SeniorityLevel entities via stored procedures using the shared data access layer.</summary>
    public class SeniorityLevelRepository : ISeniorityLevelRepository
    {
        private readonly ISharedRepository _shared;

        /// <summary>Initializes a new instance of the <see cref="SeniorityLevelRepository"/> class.</summary>
        public SeniorityLevelRepository(ISharedRepository shared)
        {
            _shared = shared;
        }

        /// <summary>Executes config.web_get_seniority_levels with pagination and maps the result set to SeniorityLevel objects.</summary>
        public async Task<IEnumerable<SeniorityLevel>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@page_number", page_number),
                new("@rows_page", rows_page)
            ];

            return await _shared.QueryAsync<SeniorityLevel>("config.web_get_seniority_levels", parameters);
        }

        /// <summary>Executes config.web_get_seniority_level_by_id and returns the first matching record.</summary>
        public async Task<SeniorityLevel?> GetByIdAsync(int id)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_seniority_level_id", id)
            ];

            var result = await _shared.QueryAsync<SeniorityLevel>("config.web_get_seniority_level_by_id", parameters);
            return result.FirstOrDefault();
        }

        /// <summary>Executes config.web_ins_seniority_level and returns the newly generated identifier.</summary>
        public async Task<int> CreateAsync(int organizationId, string seniorityName, int? sortOrder, int createdBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@p_seniority_name", seniorityName),
                new("@p_sort_order", sortOrder),
                new("@p_created_by", createdBy)
            ];

            object? result = await _shared.QueryScalarAsync("config.web_ins_seniority_level", parameters);
            return Convert.ToInt32(result);
        }

        /// <summary>Executes config.web_upd_seniority_level to update an existing record.</summary>
        public async Task UpdateAsync(int id, string seniorityName, int? sortOrder, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_seniority_level_id", id),
                new("@p_seniority_name", seniorityName),
                new("@p_sort_order", sortOrder),
                new("@p_updated_by", updatedBy)
            ];

            await _shared.ExecuteAsync("config.web_upd_seniority_level", parameters);
        }

        /// <summary>Executes config.web_del_seniority_level to soft-delete (disable) a record.</summary>
        /// <param name="id">The seniority level identifier to disable.</param>
        /// <param name="updatedBy">The identifier of the user performing the operation.</param>
        public async Task SoftDeleteAsync(int id, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_seniority_level_id", id),
                new("@p_updated_by", updatedBy)
            ];

            await _shared.ExecuteAsync("config.web_del_seniority_level", parameters);
        }
    }
}
