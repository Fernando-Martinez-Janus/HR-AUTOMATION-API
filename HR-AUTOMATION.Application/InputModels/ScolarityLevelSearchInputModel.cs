using Shared.Kernel.InputModels;

namespace HR_AUTOMATION.Application.InputModels
{
    public class ScolarityLevelSearchInputModel : PaginationRequest
    {
        public int? OrganizationId { get; set; }

        public override void Normalize()
        {
            base.Normalize();
        }
    }
}
