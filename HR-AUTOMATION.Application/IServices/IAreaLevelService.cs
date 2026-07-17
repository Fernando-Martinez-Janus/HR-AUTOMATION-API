using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Provides operations for managing area levels.
    /// </summary>
    public interface IAreaLevelService
    {
        /// <summary>
        /// Retrieves area levels matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching area levels.</returns>
        Task<IEnumerable<AreaLevelViewModel>> SearchAsync(AreaLevelSearchInputModel model);

        /// <summary>
        /// Retrieves an area level by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the area level.</param>
        /// <returns>The requested <see cref="AreaLevelViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified area level does not exist.</exception>
        Task<AreaLevelViewModel> GetAsync(int id);

        /// <summary>
        /// Creates a new area level.
        /// </summary>
        /// <param name="model">The area level information.</param>
        /// <returns>The identifier of the newly created area level.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the area level cannot be created.</exception>
        Task<int> CreateAsync(AreaLevelInputModel model);

        /// <summary>
        /// Updates an existing area level.
        /// </summary>
        /// <param name="id">The identifier of the area level to update.</param>
        /// <param name="model">The updated area level information.</param>
        Task UpdateAsync(int id, AreaLevelInputModel model);

        /// <summary>
        /// Deletes an existing area level.
        /// </summary>
        /// <param name="id">The identifier of the area level to delete.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Updates the sort order of an area level within an organization.
        /// </summary>
        /// <param name="model">The reorder request information.</param>
        /// <exception cref="ResponseExceptionFactory">Thrown when the required organization information is missing or the operation fails.</exception>
        Task ReorderAsync(ReorderInputModel model);
    }
}
