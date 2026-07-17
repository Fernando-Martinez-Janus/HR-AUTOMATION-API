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
    /// Provides endpoints for managing work modalities.
    /// </summary>
    /// <param name="service">Instance of Work Modality service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Work Modalities")]
    [Route("api/v{version:apiVersion}/work-modalities")]
    public class WorkModalitiesController(IWorkModalityService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Work Modality service.
        /// </summary>
        private readonly IWorkModalityService _service = service;

        /// <summary>
        /// Retrieves work modalities matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching work modalities.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<IEnumerable<WorkModalityViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] WorkModalitySearchInputModel model)
        {
            IEnumerable<WorkModalityViewModel> result = await _service.SearchAsync(model);

            Response<IEnumerable<WorkModalityViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves a work modality by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the work modality.</param>
        /// <returns>The requested <see cref="WorkModalityViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified work modality does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<WorkModalityViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            WorkModalityViewModel result = await _service.GetAsync(id);

            Response<WorkModalityViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new work modality.
        /// </summary>
        /// <param name="model">The work modality information.</param>
        /// <returns>The identifier of the newly created work modality.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the work modality cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] WorkModalityInputModel model)
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
        /// Updates an existing work modality.
        /// </summary>
        /// <param name="id">The identifier of the work modality to update.</param>
        /// <param name="model">The updated work modality information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] WorkModalityInputModel model)
        {
            await _service.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing work modality.
        /// </summary>
        /// <param name="id">The identifier of the work modality to delete.</param>
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
        /// Updates the sort order of a work modality within an organization.
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
