using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    public interface ISkillLevelService
    {

        Task<IEnumerable<SkillLevelViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number);
        Task<SkillLevelViewModel> GetByIdAsync(int id);
        Task CreateAsync(SkillLevelInputModel skillLevelInputModel);
        Task UpdateAsync(int id, SkillLevelInputModel skillLevelInputModel);
        Task SoftDeleteAsync(int id, int updatedBy);
    }
}
