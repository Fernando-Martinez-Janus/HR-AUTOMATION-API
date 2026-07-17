using Asp.Versioning;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.ViewModels;
using HR_AUTOMATION.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Constants;
using Shared.Kernel.ViewModels;

namespace HR_AUTOMATION_API.Controllers
{
    /// <summary>
    /// Provides endpoints for managing question categories.
    /// </summary>
    /// <param name="service">Instance of Question Category service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Question Categories")]
    [Route("api/v{version:apiVersion}/question-categories")]
    public class QuestionCategoriesController(IQuestionCategoryService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Question Category service.
        /// </summary>
        private readonly IQuestionCategoryService _service = service;

        /// <summary>
        /// Retrieves question categories matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A paginated collection of matching question categories.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<PaginationResponse<QuestionCategoryViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] QuestionCategorySearchInputModel model)
        {
            PaginationResponse<QuestionCategoryViewModel> result = await _service.SearchAsync(model);

            Response<PaginationResponse<QuestionCategoryViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves a question category by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the question category.</param>
        /// <returns>The requested <see cref="QuestionCategoryViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified question category does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<QuestionCategoryViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            QuestionCategoryViewModel result = await _service.GetAsync(id);

            Response<QuestionCategoryViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new question category.
        /// </summary>
        /// <param name="model">The question category information.</param>
        /// <returns>The identifier of the newly created question category.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the question category cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] QuestionCategoryInputModel model)
        {
            int result = await _service.CreateAsync(model);

            Response<int> response = new()
            {
                Code = StatusCodes.Status201Created,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Updates an existing question category.
        /// </summary>
        /// <param name="id">The identifier of the question category to update.</param>
        /// <param name="model">The updated question category information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] QuestionCategoryInputModel model)
        {
            await _service.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing question category.
        /// </summary>
        /// <param name="id">The identifier of the question category to delete.</param>
        [HttpDelete("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }
    }
}
