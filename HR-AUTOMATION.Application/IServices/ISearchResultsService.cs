using HR_AUTOMATION.Application.InputModels;


namespace HR_AUTOMATION.Application.IServices;

public interface ISearchResultsService
{
    Task<IEnumerable<int>> CreateAsync(SearchResultsInputModel model);
}
