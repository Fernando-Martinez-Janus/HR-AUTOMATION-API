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
    /// Provides endpoints for managing area levels.
    /// </summary>
    /// <param name="service">Instance of Area Level service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Area Levels")]
    [Route("api/v{version:apiVersion}/area-levels")]
    public class AreaLevelsController(IAreaLevelService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Area Level service.
        /// </summary>
        private readonly IAreaLevelService _service = service;

        /// <summary>
        /// Retrieves area levels matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching area levels.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<IEnumerable<AreaLevelViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] AreaLevelSearchInputModel model)
        {
            IEnumerable<AreaLevelViewModel> result = await _service.SearchAsync(model);

            Response<IEnumerable<AreaLevelViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves an area level by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the area level.</param>
        /// <returns>The requested <see cref="AreaLevelViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified area level does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<AreaLevelViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            AreaLevelViewModel result = await _service.GetAsync(id);

            Response<AreaLevelViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new area level.
        /// </summary>
        /// <param name="model">The area level information.</param>
        /// <returns>The identifier of the newly created area level.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the area level cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] AreaLevelInputModel model)
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
        /// Updates an existing area level.
        /// </summary>
        /// <param name="id">The identifier of the area level to update.</param>
        /// <param name="model">The updated area level information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] AreaLevelInputModel model)
        {
            await _service.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing area level.
        /// </summary>
        /// <param name="id">The identifier of the area level to delete.</param>
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
        /// Updates the sort order of an area level within an organization.
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
