using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Provides operations for managing work modalities.
    /// </summary>
    public interface IWorkModalityService
    {
        /// <summary>
        /// Retrieves work modalities matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching work modalities.</returns>
        Task<IEnumerable<WorkModalityViewModel>> SearchAsync(WorkModalitySearchInputModel model);

        /// <summary>
        /// Retrieves a work modality by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the work modality.</param>
        /// <returns>The requested <see cref="WorkModalityViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified work modality does not exist.</exception>
        Task<WorkModalityViewModel> GetAsync(int id);

        /// <summary>
        /// Creates a new work modality.
        /// </summary>
        /// <param name="model">The work modality information.</param>
        /// <returns>The identifier of the newly created work modality.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the work modality cannot be created.</exception>
        Task<int> CreateAsync(WorkModalityInputModel model);

        /// <summary>
        /// Updates an existing work modality.
        /// </summary>
        /// <param name="id">The identifier of the work modality to update.</param>
        /// <param name="model">The updated work modality information.</param>
        Task UpdateAsync(int id, WorkModalityInputModel model);

        /// <summary>
        /// Deletes an existing work modality.
        /// </summary>
        /// <param name="id">The identifier of the work modality to delete.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Updates the sort order of a work modality within an organization.
        /// </summary>
        /// <param name="model">The reorder request information.</param>
        /// <exception cref="ResponseExceptionFactory">Thrown when the required organization information is missing or the operation fails.</exception>
        Task ReorderAsync(ReorderInputModel model);
    }
}
