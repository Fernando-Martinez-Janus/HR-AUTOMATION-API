using HR_AUTOMATION.Application.InputModels;

namespace HR_AUTOMATION.Application.IServices;

public interface IScraperService
{
    Task ScrapeAsync(ScrapeInputModel request, Guid jobId);
}