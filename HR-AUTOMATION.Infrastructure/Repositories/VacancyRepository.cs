using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Shared.Kernel.IRepositories;

namespace HR_AUTOMATION.Infrastructure.Repositories
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly ISharedRepository _shared;

        public VacancyRepository(ISharedRepository shared)
        {
            _shared = shared;
        }

        public async Task<IEnumerable<Vacancy>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@page_number", page_number),
                new("@rows_page", rows_page)
            ];
            return await _shared.QueryAsync<Vacancy>("recruitment.web_get_vacancies", parameters);
        }

        public async Task<Vacancy?> GetByIdAsync(int id)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_vacancy_id", id)
            ];
            var result = await _shared.QueryAsync<Vacancy>("recruitment.web_get_vacancy_by_id", parameters);
            return result.FirstOrDefault();
        }

        public async Task<int> CreateAsync(int? organizationId, int? profileId, int? criticalityLevelId, int? vacancyStatusId,
            string vacancyTitle, string? clientName, string? projectName, string? vacancyLocation,
            int positionCount, decimal? salaryRangeMin, decimal? salaryRangeMax,
            DateTime? requestDate, DateTime? deadlineDate,
            int? modalityId, int? contractTypeId, int? currencyId, int? payFrequencyId, string? notes,
            int createdBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@p_profile_id", profileId),
                new("@p_criticality_level_id", criticalityLevelId),
                new("@p_vacancy_status_id", vacancyStatusId),
                new("@p_vacancy_title", vacancyTitle),
                new("@p_client_name", clientName),
                new("@p_project_name", projectName),
                new("@p_vacancy_location", vacancyLocation),
                new("@p_position_count", positionCount),
                new("@p_salary_range_min", salaryRangeMin),
                new("@p_salary_range_max", salaryRangeMax),
                new("@p_request_date", requestDate),
                new("@p_deadline_date", deadlineDate),
                new("@p_modality_id", modalityId),
                new("@p_contract_type_id", contractTypeId),
                new("@p_currency_id", currencyId),
                new("@p_pay_frequency_id", payFrequencyId),
                new("@p_notes", notes),
                new("@p_created_by", createdBy)
            ];
            object? result = await _shared.QueryScalarAsync("recruitment.web_ins_vacancy", parameters);
            return Convert.ToInt32(result);
        }

        public async Task<long> UpsertAsync(int? vacancyId, int? organizationId, int? profileId, int? criticalityLevelId, int? vacancyStatusId,
            string? vacancyTitle, string? clientName, string? projectName, string? vacancyLocation,
            int? positionCount, decimal? salaryRangeMin, decimal? salaryRangeMax,
            DateTime? requestDate, DateTime? deadlineDate,
            int? modalityId, int? contractTypeId, int? currencyId, int? payFrequencyId, string? notes,
            int createdBy, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_vacancy_id", vacancyId),
                new("@p_organization_id", organizationId),
                new("@p_profile_id", profileId),
                new("@p_criticality_level_id", criticalityLevelId),
                new("@p_vacancy_status_id", vacancyStatusId),
                new("@p_vacancy_title", vacancyTitle),
                new("@p_client_name", clientName),
                new("@p_project_name", projectName),
                new("@p_vacancy_location", vacancyLocation),
                new("@p_position_count", positionCount),
                new("@p_salary_range_min", salaryRangeMin),
                new("@p_salary_range_max", salaryRangeMax),
                new("@p_request_date", requestDate),
                new("@p_deadline_date", deadlineDate),
                new("@p_modality_id", modalityId),
                new("@p_contract_type_id", contractTypeId),
                new("@p_currency_id", currencyId),
                new("@p_pay_frequency_id", payFrequencyId),
                new("@p_notes", notes),
                new("@p_created_by", createdBy),
                new("@p_updated_by", updatedBy)
            ];
            object? result = await _shared.QueryScalarAsync("recruitment.web_upsert_vacancy", parameters);
            return Convert.ToInt64(result);
        }

        public async Task UpdateAsync(int id, int? profileId, int? criticalityLevelId, int? vacancyStatusId,
            string vacancyTitle, string? clientName, string? projectName, string? vacancyLocation,
            int positionCount, decimal? salaryRangeMin, decimal? salaryRangeMax,
            DateTime? requestDate, DateTime? deadlineDate,
            int? modalityId, int? contractTypeId, int? currencyId, int? payFrequencyId, string? notes,
            int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_vacancy_id", id),
                new("@p_profile_id", profileId),
                new("@p_criticality_level_id", criticalityLevelId),
                new("@p_vacancy_status_id", vacancyStatusId),
                new("@p_vacancy_title", vacancyTitle),
                new("@p_client_name", clientName),
                new("@p_project_name", projectName),
                new("@p_vacancy_location", vacancyLocation),
                new("@p_position_count", positionCount),
                new("@p_salary_range_min", salaryRangeMin),
                new("@p_salary_range_max", salaryRangeMax),
                new("@p_request_date", requestDate),
                new("@p_deadline_date", deadlineDate),
                new("@p_modality_id", modalityId),
                new("@p_contract_type_id", contractTypeId),
                new("@p_currency_id", currencyId),
                new("@p_pay_frequency_id", payFrequencyId),
                new("@p_notes", notes),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("recruitment.web_upd_vacancy", parameters);
        }

        public async Task SoftDeleteAsync(int id, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_vacancy_id", id),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("recruitment.web_del_vacancy", parameters);
        }
    }
}
