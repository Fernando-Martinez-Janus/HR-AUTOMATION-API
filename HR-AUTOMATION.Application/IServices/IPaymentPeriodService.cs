using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Provides operations for managing payment periods.
    /// </summary>
    public interface IPaymentPeriodService
    {
        /// <summary>
        /// Retrieves payment periods matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching payment periods.</returns>
        Task<IEnumerable<PaymentPeriodViewModel>> SearchAsync(PaymentPeriodSearchInputModel model);

        /// <summary>
        /// Retrieves a payment period by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the payment period.</param>
        /// <returns>The requested <see cref="PaymentPeriodViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified payment period does not exist.</exception>
        Task<PaymentPeriodViewModel> GetAsync(int id);

        /// <summary>
        /// Creates a new payment period.
        /// </summary>
        /// <param name="model">The payment period information.</param>
        /// <returns>The identifier of the newly created payment period.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the payment period cannot be created.</exception>
        Task<int> CreateAsync(PaymentPeriodInputModel model);

        /// <summary>
        /// Updates an existing payment period.
        /// </summary>
        /// <param name="id">The identifier of the payment period to update.</param>
        /// <param name="model">The updated payment period information.</param>
        Task UpdateAsync(int id, PaymentPeriodInputModel model);

        /// <summary>
        /// Deletes an existing payment period.
        /// </summary>
        /// <param name="id">The identifier of the payment period to delete.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Updates the sort order of a payment period within an organization.
        /// </summary>
        /// <param name="model">The reorder request information.</param>
        /// <exception cref="ResponseExceptionFactory">Thrown when the required organization information is missing or the operation fails.</exception>
        Task ReorderAsync(ReorderInputModel model);
    }
}
