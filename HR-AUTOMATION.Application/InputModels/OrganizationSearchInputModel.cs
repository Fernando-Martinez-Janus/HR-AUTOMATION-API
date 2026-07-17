using Shared.Kernel.InputModels;

namespace HR_AUTOMATION.Application.InputModels
{
    public class OrganizationSearchInputModel : PaginationRequest
    {
        public new void Normalize()
        {
            base.Normalize();
        }
    }
}