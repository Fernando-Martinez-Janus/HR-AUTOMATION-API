using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Shared.Kernel.IRepositories;

namespace HR_AUTOMATION.Infrastructure.Repositories
{
    public class VacancyStatusRepository : IVacancyStatusRepository
    {
        private readonly ISharedRepository _shared;

        public VacancyStatusRepository(ISharedRepository shared)
        {
            _shared = shared;
        }

        public async Task<IEnumerable<VacancyStatus>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@page_number", page_number),
                new("@rows_page", rows_page)
            ];
            return await _shared.QueryAsync<VacancyStatus>("config.web_get_vacancy_statuses", parameters);
        }

        public async Task<VacancyStatus?> GetByIdAsync(int id)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_vacancy_status_id", id)
            ];
            var result = await _shared.QueryAsync<VacancyStatus>("config.web_get_vacancy_status_by_id", parameters);
            return result.FirstOrDefault();
        }

        public async Task<int> CreateAsync(int organizationId, string statusName, string statusDescription, int sortOrder, int createdBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@p_status_name", statusName),
                new("@p_status_description", statusDescription),
                new("@p_sort_order", sortOrder),
                new("@p_created_by", createdBy)
            ];
            object? result = await _shared.QueryScalarAsync("config.web_ins_vacancy_status", parameters);
            return Convert.ToInt32(result);
        }

        public async Task UpdateAsync(int id, string statusName, string statusDescription, int sortOrder, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_vacancy_status_id", id),
                new("@p_status_name", statusName),
                new("@p_status_description", statusDescription),
                new("@p_sort_order", sortOrder),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("config.web_upd_vacancy_status", parameters);
        }

        public async Task SoftDeleteAsync(int id, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_vacancy_status_id", id),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("config.web_del_vacancy_status", parameters);
        }
    }
}
