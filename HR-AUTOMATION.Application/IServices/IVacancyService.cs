using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;
using Shared.Kernel.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    public interface IVacancyService
    {
        Task<PaginationResponse<VacancyViewModel>> SearchAsync(VacancySearchInputModel model);
        Task<VacancyViewModel> GetAsync(int id);
        Task<int> CreateAsync(VacancyInputModel model);
        Task<VacancyViewModel> UpsertAsync(VacancyInputModel model);
        Task UpdateAsync(int id, VacancyInputModel model);
        Task DeleteAsync(int id);
    }
}
