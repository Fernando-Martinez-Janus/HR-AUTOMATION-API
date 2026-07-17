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
    /// Provides endpoints for managing criticality levels.
    /// </summary>
    /// <param name="service">Instance of Criticality Level service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Criticality Levels")]
    [Route("api/v{version:apiVersion}/criticality-levels")]
    public class CriticalityLevelsController(ICriticalityLevelService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Criticality Level service.
        /// </summary>
        private readonly ICriticalityLevelService _service = service;

        /// <summary>
        /// Retrieves criticality levels matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching criticality levels.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<IEnumerable<CriticalityLevelViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] CriticalityLevelSearchInputModel model)
        {
            IEnumerable<CriticalityLevelViewModel> result = await _service.SearchAsync(model);

            Response<IEnumerable<CriticalityLevelViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves a criticality level by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the criticality level.</param>
        /// <returns>The requested <see cref="CriticalityLevelViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified criticality level does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<CriticalityLevelViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            CriticalityLevelViewModel result = await _service.GetAsync(id);

            Response<CriticalityLevelViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new criticality level.
        /// </summary>
        /// <param name="model">The criticality level information.</param>
        /// <returns>The identifier of the newly created criticality level.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the criticality level cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CriticalityLevelInputModel model)
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
        /// Updates an existing criticality level.
        /// </summary>
        /// <param name="id">The identifier of the criticality level to update.</param>
        /// <param name="model">The updated criticality level information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] CriticalityLevelInputModel model)
        {
            await _service.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing criticality level.
        /// </summary>
        /// <param name="id">The identifier of the criticality level to delete.</param>
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
        /// Updates the sort order of a criticality level within an organization.
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
