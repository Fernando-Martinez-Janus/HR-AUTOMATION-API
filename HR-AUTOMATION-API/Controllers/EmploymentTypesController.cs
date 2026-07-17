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
    /// Provides endpoints for managing employment types.
    /// </summary>
    /// <param name="service">Instance of Employment Type service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Employment Types")]
    [Route("api/v{version:apiVersion}/employment-types")]
    public class EmploymentTypesController(IEmploymentTypeService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Employment Type service.
        /// </summary>
        private readonly IEmploymentTypeService _service = service;

        /// <summary>
        /// Retrieves employment types matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching employment types.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<IEnumerable<EmploymentTypeViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] EmploymentTypeSearchInputModel model)
        {
            IEnumerable<EmploymentTypeViewModel> result = await _service.SearchAsync(model);

            Response<IEnumerable<EmploymentTypeViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves an employment type by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the employment type.</param>
        /// <returns>The requested <see cref="EmploymentTypeViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified employment type does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<EmploymentTypeViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            EmploymentTypeViewModel result = await _service.GetAsync(id);

            Response<EmploymentTypeViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new employment type.
        /// </summary>
        /// <param name="model">The employment type information.</param>
        /// <returns>The identifier of the newly created employment type.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the employment type cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] EmploymentTypeInputModel model)
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
        /// Updates an existing employment type.
        /// </summary>
        /// <param name="id">The identifier of the employment type to update.</param>
        /// <param name="model">The updated employment type information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] EmploymentTypeInputModel model)
        {
            await _service.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing employment type.
        /// </summary>
        /// <param name="id">The identifier of the employment type to delete.</param>
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
        /// Updates the sort order of an employment type within an organization.
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
