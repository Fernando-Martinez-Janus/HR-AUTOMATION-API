using Shared.Kernel.InputModels;

namespace HR_AUTOMATION.Application.InputModels
{
    public class SourcingResultSearchInputModel : PaginationRequest
    {
        /// <summary>
        /// Normalizes the filter values.
        /// </summary>
        /// <remarks>
        /// Calls the base <see cref="PaginationRequest.Normalize"/> method.
        /// Override this method to add custom normalization logic for user filters.
        /// </remarks>
        public override void Normalize()
        {
            base.Normalize();
        }
    }
}
