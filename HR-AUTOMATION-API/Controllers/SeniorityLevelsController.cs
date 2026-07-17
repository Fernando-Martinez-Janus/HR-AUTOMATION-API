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
    /// Provides endpoints for managing seniority levels.
    /// </summary>
    /// <param name="service">Instance of Seniority Level service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Seniority Levels")]
    [Route("api/v{version:apiVersion}/seniority-levels")]
    public class SeniorityLevelsController(ISeniorityLevelService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Seniority Level service.
        /// </summary>
        private readonly ISeniorityLevelService _service = service;

        /// <summary>
        /// Retrieves seniority levels matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching seniority levels.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<IEnumerable<SeniorityLevelViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] SeniorityLevelSearchInputModel model)
        {
            IEnumerable<SeniorityLevelViewModel> result = await _service.SearchAsync(model);

            Response<IEnumerable<SeniorityLevelViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves a seniority level by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the seniority level.</param>
        /// <returns>The requested <see cref="SeniorityLevelViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified seniority level does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<SeniorityLevelViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            SeniorityLevelViewModel result = await _service.GetAsync(id);

            Response<SeniorityLevelViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new seniority level.
        /// </summary>
        /// <param name="model">The seniority level information.</param>
        /// <returns>The identifier of the newly created seniority level.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the seniority level cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] SeniorityLevelInputModel model)
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
        /// Updates an existing seniority level.
        /// </summary>
        /// <param name="id">The identifier of the seniority level to update.</param>
        /// <param name="model">The updated seniority level information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] SeniorityLevelInputModel model)
        {
            await _service.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing seniority level.
        /// </summary>
        /// <param name="id">The identifier of the seniority level to delete.</param>
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
        /// Updates the sort order of a seniority level within an organization.
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
