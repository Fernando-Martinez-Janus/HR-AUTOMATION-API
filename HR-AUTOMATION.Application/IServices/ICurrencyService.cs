using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Provides operations for managing currencies.
    /// </summary>
    public interface ICurrencyService
    {
        /// <summary>
        /// Retrieves currencies matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching currencies.</returns>
        Task<IEnumerable<CurrencyViewModel>> SearchAsync(CurrencySearchInputModel model);

        /// <summary>
        /// Retrieves a currency by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the currency.</param>
        /// <returns>The requested <see cref="CurrencyViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified currency does not exist.</exception>
        Task<CurrencyViewModel> GetAsync(int id);

        /// <summary>
        /// Creates a new currency.
        /// </summary>
        /// <param name="model">The currency information.</param>
        /// <returns>The identifier of the newly created currency.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the currency cannot be created.</exception>
        Task<int> CreateAsync(CurrencyInputModel model);

        /// <summary>
        /// Updates an existing currency.
        /// </summary>
        /// <param name="id">The identifier of the currency to update.</param>
        /// <param name="model">The updated currency information.</param>
        Task UpdateAsync(int id, CurrencyInputModel model);

        /// <summary>
        /// Deletes an existing currency.
        /// </summary>
        /// <param name="id">The identifier of the currency to delete.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Updates the sort order of a currency within an organization.
        /// </summary>
        /// <param name="model">The reorder request information.</param>
        /// <exception cref="ResponseExceptionFactory">Thrown when the required organization information is missing or the operation fails.</exception>
        Task ReorderAsync(ReorderInputModel model);
    }
}
