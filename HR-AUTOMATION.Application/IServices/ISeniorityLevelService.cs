using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Provides operations for managing seniority levels.
    /// </summary>
    public interface ISeniorityLevelService
    {
        /// <summary>
        /// Retrieves seniority levels matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching seniority levels.</returns>
        Task<IEnumerable<SeniorityLevelViewModel>> SearchAsync(SeniorityLevelSearchInputModel model);

        /// <summary>
        /// Retrieves a seniority level by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the seniority level.</param>
        /// <returns>The requested <see cref="SeniorityLevelViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified seniority level does not exist.</exception>
        Task<SeniorityLevelViewModel> GetAsync(int id);

        /// <summary>
        /// Creates a new seniority level.
        /// </summary>
        /// <param name="model">The seniority level information.</param>
        /// <returns>The identifier of the newly created seniority level.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the seniority level cannot be created.</exception>
        Task<int> CreateAsync(SeniorityLevelInputModel model);

        /// <summary>
        /// Updates an existing seniority level.
        /// </summary>
        /// <param name="id">The identifier of the seniority level to update.</param>
        /// <param name="model">The updated seniority level information.</param>
        Task UpdateAsync(int id, SeniorityLevelInputModel model);

        /// <summary>
        /// Deletes an existing seniority level.
        /// </summary>
        /// <param name="id">The identifier of the seniority level to delete.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Updates the sort order of a seniority level within an organization.
        /// </summary>
        /// <param name="model">The reorder request information.</param>
        /// <exception cref="ResponseExceptionFactory">Thrown when the required organization information is missing or the operation fails.</exception>
        Task ReorderAsync(ReorderInputModel model);
    }
}
