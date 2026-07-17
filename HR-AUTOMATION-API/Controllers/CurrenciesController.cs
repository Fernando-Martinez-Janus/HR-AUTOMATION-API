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
    /// Provides endpoints for managing currencies.
    /// </summary>
    /// <param name="service">Instance of Currency service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Currencies")]
    [Route("api/v{version:apiVersion}/currencies")]
    public class CurrenciesController(ICurrencyService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Currency service.
        /// </summary>
        private readonly ICurrencyService _service = service;

        /// <summary>
        /// Retrieves currencies matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching currencies.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<IEnumerable<CurrencyViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] CurrencySearchInputModel model)
        {
            IEnumerable<CurrencyViewModel> result = await _service.SearchAsync(model);

            Response<IEnumerable<CurrencyViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves a currency by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the currency.</param>
        /// <returns>The requested <see cref="CurrencyViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified currency does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<CurrencyViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            CurrencyViewModel result = await _service.GetAsync(id);

            Response<CurrencyViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new currency.
        /// </summary>
        /// <param name="model">The currency information.</param>
        /// <returns>The identifier of the newly created currency.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the currency cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CurrencyInputModel model)
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
        /// Updates an existing currency.
        /// </summary>
        /// <param name="id">The identifier of the currency to update.</param>
        /// <param name="model">The updated currency information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] CurrencyInputModel model)
        {
            await _service.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing currency.
        /// </summary>
        /// <param name="id">The identifier of the currency to delete.</param>
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
        /// Updates the sort order of a currency within an organization.
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
