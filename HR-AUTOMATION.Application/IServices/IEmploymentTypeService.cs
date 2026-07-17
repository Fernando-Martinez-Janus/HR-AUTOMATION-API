using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Provides operations for managing employment types.
    /// </summary>
    public interface IEmploymentTypeService
    {
        /// <summary>
        /// Retrieves employment types matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching employment types.</returns>
        Task<IEnumerable<EmploymentTypeViewModel>> SearchAsync(EmploymentTypeSearchInputModel model);

        /// <summary>
        /// Retrieves an employment type by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the employment type.</param>
        /// <returns>The requested <see cref="EmploymentTypeViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified employment type does not exist.</exception>
        Task<EmploymentTypeViewModel> GetAsync(int id);

        /// <summary>
        /// Creates a new employment type.
        /// </summary>
        /// <param name="model">The employment type information.</param>
        /// <returns>The identifier of the newly created employment type.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the employment type cannot be created.</exception>
        Task<int> CreateAsync(EmploymentTypeInputModel model);

        /// <summary>
        /// Updates an existing employment type.
        /// </summary>
        /// <param name="id">The identifier of the employment type to update.</param>
        /// <param name="model">The updated employment type information.</param>
        Task UpdateAsync(int id, EmploymentTypeInputModel model);

        /// <summary>
        /// Deletes an existing employment type.
        /// </summary>
        /// <param name="id">The identifier of the employment type to delete.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Updates the sort order of an employment type within an organization.
        /// </summary>
        /// <param name="model">The reorder request information.</param>
        /// <exception cref="ResponseExceptionFactory">Thrown when the required organization information is missing or the operation fails.</exception>
        Task ReorderAsync(ReorderInputModel model);
    }
}
