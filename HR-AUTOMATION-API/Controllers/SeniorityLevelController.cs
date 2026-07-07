using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;

namespace HR_AUTOMATION_API.Controllers
{
    /// <summary>Handles HTTP requests for seniority level operations.</summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/seniority-levels")]
    [EnableRateLimiting("general")]
    public class SeniorityLevelController : ControllerBase
    {
        private readonly ISeniorityLevelService _seniorityLevelService;

        /// <summary>Initializes a new instance of the <see cref="SeniorityLevelController"/> class.</summary>
        public SeniorityLevelController(ISeniorityLevelService seniorityLevelService)
        {
            _seniorityLevelService = seniorityLevelService;
        }

        /// <summary>Retrieves all active seniority levels for an organization with pagination.</summary>
        /// <param name="organizationId">The organization identifier (query parameter).</param>
        /// <param name="rows_page">Number of rows per page (query parameter).</param>
        /// <param name="page_number">Page number to retrieve (query parameter).</param>
        /// <returns>A 200 response with a list of seniority levels.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAll([FromQuery] int organizationId, [FromQuery] int rows_page, [FromQuery] int page_number)
        {
            var result = await _seniorityLevelService.GetAllAsync(organizationId, rows_page, page_number);
            var response = new Response<IEnumerable<SeniorityLevelViewModel>>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return Ok(response);
        }

        /// <summary>Retrieves a single seniority level by its identifier.</summary>
        /// <param name="id">The seniority level identifier (path parameter).</param>
        /// <returns>A 200 response with the record, or 404 if not found.</returns>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _seniorityLevelService.GetByIdAsync(id);
            if (result == null) return NotFound();

            var response = new Response<SeniorityLevelViewModel>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return Ok(response);
        }

        /// <summary>Creates a new seniority level.</summary>
        /// <param name="input">The input data for the new record.</param>
        /// <returns>A 201 response with the newly generated identifier.</returns>
        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] SeniorityLevelInputModel input)
        {
            var result = await _seniorityLevelService.CreateAsync(input);
            var response = new Response<int>
            {
                Code = StatusCodes.Status201Created,
                DataResponse = result.Id
            };
            return StatusCode(StatusCodes.Status201Created, response);
        }

        /// <summary>Updates an existing seniority level.</summary>
        /// <param name="id">The identifier of the record to update.</param>
        /// <param name="input">The updated input data.</param>
        /// <returns>A 204 response on success, or 404 if not found.</returns>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(int id, [FromBody] SeniorityLevelInputModel input)
        {
            await _seniorityLevelService.UpdateAsync(id, input);
            return NoContent();
        }

        /// <summary>Soft-deletes a seniority level.</summary>
        /// <param name="id">The identifier of the record to disable.</param>
        /// <param name="updatedBy">The identifier of the user performing the operation (query parameter).</param>
        /// <returns>A 204 response on success, or 404 if not found.</returns>
        [HttpDelete("{id:int}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int updatedBy)
        {
            await _seniorityLevelService.DeleteAsync(id, updatedBy);
            return NoContent();
        }
    }
}
