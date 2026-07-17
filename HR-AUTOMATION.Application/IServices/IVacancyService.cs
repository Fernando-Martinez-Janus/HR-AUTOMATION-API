using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;
using Shared.Kernel.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Defines the contract for vacancy-related business operations.
    /// </summary>
    public interface IVacancyService
    {
        /// <summary>
        /// Searches for vacancies based on the specified criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A paginated response containing the matching vacancies.</returns>
        Task<PaginationResponse<VacancyViewModel>> SearchAsync(VacancySearchInputModel model);

        /// <summary>
        /// Retrieves a vacancy by its identifier.
        /// </summary>
        /// <param name="id">The vacancy identifier.</param>
        /// <returns>The <see cref="VacancyViewModel"/> if found; otherwise throws <see cref="ResponseExceptionFactory"/>.</returns>
        Task<VacancyViewModel> GetAsync(int id);

        /// <summary>
        /// Creates a new vacancy.
        /// </summary>
        /// <param name="model">The vacancy information.</param>
        /// <returns>The identifier of the newly created vacancy.</returns>
        Task<int> CreateAsync(VacancyInputModel model);

        /// <summary>
        /// Creates or updates a vacancy (draft). If the vacancy identifier is provided, the existing vacancy is updated; otherwise, a new one is created.
        /// </summary>
        /// <param name="model">The vacancy information.</param>
        /// <returns>The <see cref="VacancyViewModel"/> of the upserted vacancy.</returns>
        Task<VacancyViewModel> UpsertAsync(VacancyInputModel model);

        /// <summary>
        /// Updates an existing vacancy.
        /// </summary>
        /// <param name="id">The identifier of the vacancy to update.</param>
        /// <param name="model">The updated vacancy information.</param>
        Task UpdateAsync(int id, VacancyInputModel model);

        /// <summary>
        /// Deletes an existing vacancy.
        /// </summary>
        /// <param name="id">The identifier of the vacancy to delete.</param>
        Task DeleteAsync(int id);
    }
}
