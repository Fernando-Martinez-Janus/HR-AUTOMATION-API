using Shared.Kernel.InputModels;

namespace HR_AUTOMATION.Application.InputModels
{
    public class VacancySearchInputModel : PaginationRequest
    {
        public int? OrganizationId { get; set; }
    }
}
