public interface IVacancyStatusService
{
    Task<VacancyStatusViewModel> CreateAsync(VacancyStatusInputModel input);
    Task<IEnumerable<VacancyStatusViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number);
    Task<VacancyStatusViewModel?> GetByIdAsync(int id);
    Task UpdateAsync(int id, VacancyStatusInputModel input);
    Task DeleteAsync(int id, int updatedBy);
}
