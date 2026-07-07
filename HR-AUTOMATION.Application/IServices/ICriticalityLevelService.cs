using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    public interface ICriticalityLevelService
    {

        Task<IEnumerable<CriticalityLevelViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number);
        Task<CriticalityLevelViewModel> GetByIdAsync(int id);
        Task CreateAsync(CriticalityLevelInputModel criticalityLevelInputModel);
        Task UpdateAsync(int id, CriticalityLevelInputModel criticalityLevelInputModel);
        Task SoftDeleteAsync(int id, int updatedBy);
    }
}
