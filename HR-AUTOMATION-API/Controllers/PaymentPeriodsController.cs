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
    /// Provides endpoints for managing payment periods.
    /// </summary>
    /// <param name="service">Instance of Payment Period service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Payment Periods")]
    [Route("api/v{version:apiVersion}/payment-periods")]
    public class PaymentPeriodsController(IPaymentPeriodService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Payment Period service.
        /// </summary>
        private readonly IPaymentPeriodService _service = service;

        /// <summary>
        /// Retrieves payment periods matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching payment periods.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<IEnumerable<PaymentPeriodViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] PaymentPeriodSearchInputModel model)
        {
            IEnumerable<PaymentPeriodViewModel> result = await _service.SearchAsync(model);

            Response<IEnumerable<PaymentPeriodViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves a payment period by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the payment period.</param>
        /// <returns>The requested <see cref="PaymentPeriodViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified payment period does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<PaymentPeriodViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            PaymentPeriodViewModel result = await _service.GetAsync(id);

            Response<PaymentPeriodViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new payment period.
        /// </summary>
        /// <param name="model">The payment period information.</param>
        /// <returns>The identifier of the newly created payment period.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the payment period cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] PaymentPeriodInputModel model)
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
        /// Updates an existing payment period.
        /// </summary>
        /// <param name="id">The identifier of the payment period to update.</param>
        /// <param name="model">The updated payment period information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentPeriodInputModel model)
        {
            await _service.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing payment period.
        /// </summary>
        /// <param name="id">The identifier of the payment period to delete.</param>
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
        /// Updates the sort order of a payment period within an organization.
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
