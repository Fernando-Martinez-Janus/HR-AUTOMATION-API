using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;


namespace HR_AUTOMATION.Application.IServices;

public interface ISearchResultsService
{
    Task<IEnumerable<int>> CreateAsync(SearchResultsInputModel model);

    Task<ActiveSearchViewModel> GetByVacancyAsync(int vacancyId, SourcingResultSearchInputModel model);

    Task<SearchResultViewModel> GetDetailByVacancyAsync(int vacancyId, int resultId);

    Task<IEnumerable<SearchResultViewModel>> GetTopCandidatesAsync(int topCount = 3);
}
