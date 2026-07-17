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
    /// Provides endpoints for managing skill levels.
    /// </summary>
    /// <param name="service">Instance of Skill Level service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Skill Levels")]
    [Route("api/v{version:apiVersion}/skill-levels")]
    public class SkillLevelsController(ISkillLevelService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Skill Level service.
        /// </summary>
        private readonly ISkillLevelService _service = service;

        /// <summary>
        /// Retrieves skill levels matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching skill levels.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<IEnumerable<SkillLevelViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] SkillLevelSearchInputModel model)
        {
            IEnumerable<SkillLevelViewModel> result = await _service.SearchAsync(model);

            Response<IEnumerable<SkillLevelViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves a skill level by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the skill level.</param>
        /// <returns>The requested <see cref="SkillLevelViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified skill level does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<SkillLevelViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            SkillLevelViewModel result = await _service.GetAsync(id);

            Response<SkillLevelViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new skill level.
        /// </summary>
        /// <param name="model">The skill level information.</param>
        /// <returns>The identifier of the newly created skill level.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the skill level cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] SkillLevelInputModel model)
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
        /// Updates an existing skill level.
        /// </summary>
        /// <param name="id">The identifier of the skill level to update.</param>
        /// <param name="model">The updated skill level information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] SkillLevelInputModel model)
        {
            await _service.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing skill level.
        /// </summary>
        /// <param name="id">The identifier of the skill level to delete.</param>
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
        /// Updates the sort order of a skill level within an organization.
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
