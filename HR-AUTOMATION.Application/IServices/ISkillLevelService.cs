using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Provides operations for managing skill levels.
    /// </summary>
    public interface ISkillLevelService
    {
        /// <summary>
        /// Retrieves skill levels matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching skill levels.</returns>
        Task<IEnumerable<SkillLevelViewModel>> SearchAsync(SkillLevelSearchInputModel model);

        /// <summary>
        /// Retrieves a skill level by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the skill level.</param>
        /// <returns>The requested <see cref="SkillLevelViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified skill level does not exist.</exception>
        Task<SkillLevelViewModel> GetAsync(int id);

        /// <summary>
        /// Creates a new skill level.
        /// </summary>
        /// <param name="model">The skill level information.</param>
        /// <returns>The identifier of the newly created skill level.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the skill level cannot be created.</exception>
        Task<int> CreateAsync(SkillLevelInputModel model);

        /// <summary>
        /// Updates an existing skill level.
        /// </summary>
        /// <param name="id">The identifier of the skill level to update.</param>
        /// <param name="model">The updated skill level information.</param>
        Task UpdateAsync(int id, SkillLevelInputModel model);

        /// <summary>
        /// Deletes an existing skill level.
        /// </summary>
        /// <param name="id">The identifier of the skill level to delete.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Updates the sort order of a skill level within an organization.
        /// </summary>
        /// <param name="model">The reorder request information.</param>
        /// <exception cref="ResponseExceptionFactory">Thrown when the required organization information is missing or the operation fails.</exception>
        Task ReorderAsync(ReorderInputModel model);
    }
}
