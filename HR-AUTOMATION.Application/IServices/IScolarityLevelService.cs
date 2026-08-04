using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    public interface IScolarityLevelService
    {
        Task<IEnumerable<ScolarityLevelViewModel>> SearchAsync(ScolarityLevelSearchInputModel model);
        Task<ScolarityLevelViewModel> GetAsync(int id);
        Task<int> CreateAsync(ScolarityLevelInputModel model);
        Task UpdateAsync(int id, ScolarityLevelInputModel model);
        Task DeleteAsync(int id);
        Task ReorderAsync(ReorderInputModel model);
    }
}
