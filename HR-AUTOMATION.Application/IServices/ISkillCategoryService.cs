using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Provides operations for managing skill categories.
    /// </summary>
    public interface ISkillCategoryService
    {
        /// <summary>
        /// Retrieves skill categories matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching skill categories.</returns>
        Task<IEnumerable<SkillCategoryViewModel>> SearchAsync(SkillCategorySearchInputModel model);

        /// <summary>
        /// Retrieves a skill category by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the skill category.</param>
        /// <returns>The requested <see cref="SkillCategoryViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified skill category does not exist.</exception>
        Task<SkillCategoryViewModel> GetAsync(int id);

        /// <summary>
        /// Creates a new skill category.
        /// </summary>
        /// <param name="model">The skill category information.</param>
        /// <returns>The identifier of the newly created skill category.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the skill category cannot be created.</exception>
        Task<int> CreateAsync(SkillCategoryInputModel model);

        /// <summary>
        /// Updates an existing skill category
        /// </summary>
        /// <param name="id">The identifier of the skill category to update.</param>
        /// <param name="model">The updated skill category information.</param>
        Task UpdateAsync(int id, SkillCategoryInputModel model);

        /// <summary>
        /// Deletes an existing skill category.
        /// </summary>
        /// <param name="id">The identifier of the skill category to delete.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Updates the sort order of a skill category within an organization.
        /// </summary>
        /// <param name="model">The reorder request information.</param>
        /// <exception cref="ResponseExceptionFactory">Thrown when the required organization information is missing or the operation fails.</exception>
        Task ReorderAsync(ReorderInputModel model);
    }
}