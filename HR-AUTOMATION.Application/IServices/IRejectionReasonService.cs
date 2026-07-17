using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;
using Shared.Kernel.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Provides operations for managing rejection reasons.
    /// </summary>
    public interface IRejectionReasonService
    {
        /// <summary>
        /// Retrieves rejection reasons matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A paginated collection of matching rejection reasons.</returns>
        Task<PaginationResponse<RejectionReasonViewModel>> SearchAsync(RejectionReasonSearchInputModel model);

        /// <summary>
        /// Retrieves a rejection reason by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the rejection reason.</param>
        /// <returns>The requested <see cref="RejectionReasonViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified rejection reason does not exist.</exception>
        Task<RejectionReasonViewModel> GetAsync(int id);

        /// <summary>
        /// Creates a new rejection reason.
        /// </summary>
        /// <param name="model">The rejection reason information.</param>
        /// <returns>The identifier of the newly created rejection reason.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the rejection reason cannot be created.</exception>
        Task<int> CreateAsync(RejectionReasonInputModel model);

        /// <summary>
        /// Updates an existing rejection reason.
        /// </summary>
        /// <param name="id">The identifier of the rejection reason to update.</param>
        /// <param name="model">The updated rejection reason information.</param>
        Task UpdateAsync(int id, RejectionReasonInputModel model);

        /// <summary>
        /// Deletes an existing rejection reason.
        /// </summary>
        /// <param name="id">The identifier of the rejection reason to delete.</param>
        Task DeleteAsync(int id);
    }
}
