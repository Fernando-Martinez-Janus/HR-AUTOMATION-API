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
    /// Provides endpoints for managing scolarity levels.
    /// </summary>
    /// <param name="service">Instance of Scolarity Level service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Scolarity Levels")]
    [Route("api/v{version:apiVersion}/scolarity-levels")]
    public class ScolarityLevelsController(IScolarityLevelService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Scolarity Level service.
        /// </summary>
        private readonly IScolarityLevelService _service = service;

        /// <summary>
        /// Retrieves scolarity levels matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching scolarity levels.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<IEnumerable<ScolarityLevelViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] ScolarityLevelSearchInputModel model)
        {
            IEnumerable<ScolarityLevelViewModel> result = await _service.SearchAsync(model);

            Response<IEnumerable<ScolarityLevelViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves a scolarity level by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the scolarity level.</param>
        /// <returns>The requested <see cref="ScolarityLevelViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified scolarity level does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<ScolarityLevelViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            ScolarityLevelViewModel result = await _service.GetAsync(id);

            Response<ScolarityLevelViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new scolarity level.
        /// </summary>
        /// <param name="model">The scolarity level information.</param>
        /// <returns>The identifier of the newly created scolarity level.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the scolarity level cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] ScolarityLevelInputModel model)
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
        /// Updates an existing scolarity level.
        /// </summary>
        /// <param name="id">The identifier of the scolarity level to update.</param>
        /// <param name="model">The updated scolarity level information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] ScolarityLevelInputModel model)
        {
            await _service.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing scolarity level.
        /// </summary>
        /// <param name="id">The identifier of the scolarity level to delete.</param>
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
        /// Updates the sort order of a scolarity level within an organization.
        /// </summary>
        /// <param name="model">The reorder request information.</param>
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
