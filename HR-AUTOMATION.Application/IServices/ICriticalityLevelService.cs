using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Provides operations for managing criticality levels.
    /// </summary>
    public interface ICriticalityLevelService
    {
        /// <summary>
        /// Retrieves criticality levels matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching criticality levels.</returns>
        Task<IEnumerable<CriticalityLevelViewModel>> SearchAsync(CriticalityLevelSearchInputModel model);

        /// <summary>
        /// Retrieves a criticality level by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the criticality level.</param>
        /// <returns>The requested <see cref="CriticalityLevelViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified criticality level does not exist.</exception>
        Task<CriticalityLevelViewModel> GetAsync(int id);

        /// <summary>
        /// Creates a new criticality level.
        /// </summary>
        /// <param name="model">The criticality level information.</param>
        /// <returns>The identifier of the newly created criticality level.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the criticality level cannot be created.</exception>
        Task<int> CreateAsync(CriticalityLevelInputModel model);

        /// <summary>
        /// Updates an existing criticality level.
        /// </summary>
        /// <param name="id">The identifier of the criticality level to update.</param>
        /// <param name="model">The updated criticality level information.</param>
        Task UpdateAsync(int id, CriticalityLevelInputModel model);

        /// <summary>
        /// Deletes an existing criticality level.
        /// </summary>
        /// <param name="id">The identifier of the criticality level to delete.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Updates the sort order of a criticality level within an organization.
        /// </summary>
        /// <param name="model">The reorder request information.</param>
        /// <exception cref="ResponseExceptionFactory">Thrown when the required organization information is missing or the operation fails.</exception>
        Task ReorderAsync(ReorderInputModel model);
    }
}
