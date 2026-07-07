using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    public interface IAreaLevelService
    {

        Task<IEnumerable<AreaLevelViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number);
        Task<AreaLevelViewModel> GetByIdAsync(int id);
        Task CreateAsync(AreaLevelInputModel areaLevelInputModel);
        Task UpdateAsync(int id, AreaLevelInputModel areaLevelInputModel);
        Task SoftDeleteAsync(int id, int updatedBy);
    }
}
