using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    public interface ISearchRequestService
    {
        Task<IEnumerable<SearchRequestViewModel>> SearchAsync(SearchRequestSearchInputModel model);
        Task<int> CreateAsync(SearchRequestInputModel model);
    }
}
