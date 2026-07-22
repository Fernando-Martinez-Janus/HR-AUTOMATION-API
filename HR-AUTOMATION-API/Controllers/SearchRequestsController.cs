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
    /// Provides endpoints for managing search requests.
    /// </summary>
    /// <param name="service">Instance of Search Request service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Search Requests")]
    [Route("api/v{version:apiVersion}/search-requests")]
    public class SearchRequestsController(ISearchRequestService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Search Request service.
        /// </summary>
        private readonly ISearchRequestService _service = service;

        /// <summary>
        /// Retrieves search requests matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A collection of matching search requests.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<IEnumerable<SearchRequestViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] SearchRequestSearchInputModel model)
        {
            IEnumerable<SearchRequestViewModel> result = await _service.SearchAsync(model);

            Response<IEnumerable<SearchRequestViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new search request.
        /// </summary>
        /// <param name="model">The search request information.</param>
        /// <returns>The identifier of the newly created search request.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the search request cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] SearchRequestInputModel model)
        {
            int result = await _service.CreateAsync(model);

            Response<int> response = new()
            {
                Code = StatusCodes.Status201Created,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }
    }
}
