using Shared.Kernel.InputModels;

namespace HR_AUTOMATION.Application.InputModels
{
    public class SearchRequestSearchInputModel : PaginationRequest
    {
        public int? OrganizationId { get; set; }

        public override void Normalize()
        {
            base.Normalize();
        }
    }
}
