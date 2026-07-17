using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;
using Shared.Kernel.ViewModels;

namespace HR_AUTOMATION.Application.IServices
{
    /// <summary>
    /// Provides operations for managing question categories.
    /// </summary>
    public interface IQuestionCategoryService
    {
        /// <summary>
        /// Retrieves question categories matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A paginated collection of matching question categories.</returns>
        Task<PaginationResponse<QuestionCategoryViewModel>> SearchAsync(QuestionCategorySearchInputModel model);

        /// <summary>
        /// Retrieves a question category by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the question category.</param>
        /// <returns>The requested <see cref="QuestionCategoryViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified question category does not exist.</exception>
        Task<QuestionCategoryViewModel> GetAsync(int id);

        /// <summary>
        /// Creates a new question category.
        /// </summary>
        /// <param name="model">The question category information.</param>
        /// <returns>The identifier of the newly created question category.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the question category cannot be created.</exception>
        Task<int> CreateAsync(QuestionCategoryInputModel model);

        /// <summary>
        /// Updates an existing question category.
        /// </summary>
        /// <param name="id">The identifier of the question category to update.</param>
        /// <param name="model">The updated question category information.</param>
        Task UpdateAsync(int id, QuestionCategoryInputModel model);

        /// <summary>
        /// Deletes an existing question category.
        /// </summary>
        /// <param name="id">The identifier of the question category to delete.</param>
        Task DeleteAsync(int id);
    }
}
