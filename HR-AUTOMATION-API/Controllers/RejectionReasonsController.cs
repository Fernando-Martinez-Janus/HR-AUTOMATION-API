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
    /// Provides endpoints for managing rejection reasons.
    /// </summary>
    /// <param name="service">Instance of Rejection Reason service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Rejection Reasons")]
    [Route("api/v{version:apiVersion}/rejection-reasons")]
    public class RejectionReasonsController(IRejectionReasonService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Rejection Reason service.
        /// </summary>
        private readonly IRejectionReasonService _service = service;

        /// <summary>
        /// Retrieves rejection reasons matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A paginated collection of matching rejection reasons.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<PaginationResponse<RejectionReasonViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] RejectionReasonSearchInputModel model)
        {
            PaginationResponse<RejectionReasonViewModel> result = await _service.SearchAsync(model);

            Response<PaginationResponse<RejectionReasonViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves a rejection reason by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the rejection reason.</param>
        /// <returns>The requested <see cref="RejectionReasonViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified rejection reason does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<RejectionReasonViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            RejectionReasonViewModel result = await _service.GetAsync(id);

            Response<RejectionReasonViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new rejection reason.
        /// </summary>
        /// <param name="model">The rejection reason information.</param>
        /// <returns>The identifier of the newly created rejection reason.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the rejection reason cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] RejectionReasonInputModel model)
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
        /// Updates an existing rejection reason.
        /// </summary>
        /// <param name="id">The identifier of the rejection reason to update.</param>
        /// <param name="model">The updated rejection reason information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] RejectionReasonInputModel model)
        {
            await _service.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing rejection reason.
        /// </summary>
        /// <param name="id">The identifier of the rejection reason to delete.</param>
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
