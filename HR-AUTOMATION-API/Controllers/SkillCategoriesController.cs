using Asp.Versioning;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.ViewModels;
using HR_AUTOMATION.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Constants;

namespace HR_AUTOMATION_API.Controllers
{
    /// <summary>
    /// Provides endpoints for managing skill categories.
    /// </summary>
    /// <param name="service">Instance of Skill Category service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Skill Categories")]
    [Route("api/v{version:apiVersion}/skill-categories")]
    public class SkillCategoriesController(ISkillCategoryService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Skill Category service.
        /// </summary>
        private readonly ISkillCategoryService _service = service;

        /// <summary>
        /// Retrieves skill categories matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching skill categories.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<IEnumerable<SkillCategoryViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] SkillCategorySearchInputModel model)
        {
            IEnumerable<SkillCategoryViewModel> result = await _service.SearchAsync(model);

            Response<IEnumerable<SkillCategoryViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves a skill category by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the skill category.</param>
        /// <returns>The requested <see cref="SkillCategoryViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified skill category does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<SkillCategoryViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            SkillCategoryViewModel result = await _service.GetAsync(id);

            Response<SkillCategoryViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new skill category.
        /// </summary>
        /// <param name="model">The skill category information.</param>
        /// <returns>The identifier of the newly created skill category.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the skill category cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] SkillCategoryInputModel model)
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
        /// Updates an existing skill category
        /// </summary>
        /// <param name="id">The identifier of the skill category to update.</param>
        /// <param name="model">The updated skill category information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] SkillCategoryInputModel model)
        {
            await _service.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing skill category.
        /// </summary>
        /// <param name="id">The identifier of the skill category to delete.</param>
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

        /// <summary>
        /// Updates the sort order of a skill category within an organization.
        /// </summary>
        /// <param name="model">The reorder request information.</param>
        /// <exception cref="ResponseExceptionFactory">Thrown when the required organization information is missing or the operation fails.</exception>
        [HttpPost("reorder")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Reorder(ReorderInputModel model)
        {
            await _service.ReorderAsync(model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }
    }
}