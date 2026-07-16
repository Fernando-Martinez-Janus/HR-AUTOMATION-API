using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;
using Shared.Kernel.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Provides operations for managing skill categories.
    /// </summary>
    public interface ISkillService
    {
        /// <summary>
        /// Retrieves skills matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching skills.</returns>
        Task<PaginationResponse<SkillViewModel>> SearchAsync(SkillSearchInputModel model);

        /// <summary>
        /// Retrieves a skill by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the skill.</param>
        /// <returns>The requested <see cref="SkillViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified skill does not exist.</exception>
        Task<SkillViewModel> GetAsync(int id);

        /// <summary>
        /// Creates a new skill.
        /// </summary>
        /// <param name="model">The skill information.</param>
        /// <returns>The identifier of the newly created skill.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the skill cannot be created.</exception>
        Task<int> CreateAsync(SkillInputModel model);

        /// <summary>
        /// Updates an existing skill.
        /// </summary>
        /// <param name="id">The identifier of the skill to update.</param>
        /// <param name="model">The updated skill information.</param>
        Task UpdateAsync(int id, SkillInputModel model);

        /// <summary>
        /// Deletes an existing skill.
        /// </summary>
        /// <param name="id">The identifier of the skill to delete.</param>
        Task DeleteAsync(int id);
    }
}
