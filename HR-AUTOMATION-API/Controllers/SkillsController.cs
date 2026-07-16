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
    /// Provides endpoints for managing skill categories.
    /// </summary>
    /// <param name="service">Instance of Skill service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Skills")]
    [Route("api/v{version:apiVersion}/skills")]
    public class SkillsController(ISkillService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Skill service.
        /// </summary>
        private readonly ISkillService _service = service;

        /// <summary>
        /// Retrieves skills matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching skills.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<PaginationResponse<SkillViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] SkillSearchInputModel model)
        {
            PaginationResponse<SkillViewModel> result = await _service.SearchAsync(model);

            Response<PaginationResponse<SkillViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves a skill by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the skill.</param>
        /// <returns>The requested <see cref="SkillViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified skill does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<SkillViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            SkillViewModel result = await _service.GetAsync(id);

            Response<SkillViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new skill.
        /// </summary>
        /// <param name="model">The skill information.</param>
        /// <returns>The identifier of the newly created skill.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the skill cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] SkillInputModel model)
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
        /// Updates an existing skill.
        /// </summary>
        /// <param name="id">The identifier of the skill to update.</param>
        /// <param name="model">The updated skill information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] SkillInputModel model)
        {
            await _service.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing skill.
        /// </summary>
        /// <param name="id">The identifier of the skill to delete.</param>
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
