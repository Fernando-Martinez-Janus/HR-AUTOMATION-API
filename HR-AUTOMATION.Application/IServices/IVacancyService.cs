public interface IVacancyService
{
    Task<VacancyViewModel> CreateAsync(VacancyInputModel input);
    Task<IEnumerable<VacancyViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number);
    Task<VacancyViewModel?> GetByIdAsync(int id);
    Task<VacancyViewModel> UpsertAsync(VacancyInputModel input);
    Task UpdateAsync(int id, VacancyInputModel input);
    Task DeleteAsync(int id, int updatedBy);
}
