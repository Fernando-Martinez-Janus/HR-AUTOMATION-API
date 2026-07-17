using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;
using Shared.Kernel.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Provides operations for managing organizations.
    /// </summary>
    public interface IOrganizationService
    {
        /// <summary>
        /// Retrieves organizations matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A paginated collection of matching organizations.</returns>
        Task<PaginationResponse<OrganizationViewModel>> SearchAsync(OrganizationSearchInputModel model);

        /// <summary>
        /// Retrieves an organization by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the organization.</param>
        /// <returns>The requested <see cref="OrganizationViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified organization does not exist.</exception>
        Task<OrganizationViewModel> GetAsync(int id);

        /// <summary>
        /// Creates a new organization.
        /// </summary>
        /// <param name="model">The organization information.</param>
        /// <returns>The identifier of the newly created organization.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the organization cannot be created.</exception>
        Task<int> CreateAsync(OrganizationInputModel model);

        /// <summary>
        /// Updates an existing organization.
        /// </summary>
        /// <param name="id">The identifier of the organization to update.</param>
        /// <param name="model">The updated organization information.</param>
        Task UpdateAsync(int id, OrganizationInputModel model);

        /// <summary>
        /// Deletes an existing organization.
        /// </summary>
        /// <param name="id">The identifier of the organization to delete.</param>
        Task DeleteAsync(int id);
    }
}
