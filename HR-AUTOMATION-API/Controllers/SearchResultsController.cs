using Asp.Versioning;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Constants;

namespace HR_AUTOMATION_API.Controllers
{
    /// <summary>
    /// Provides endpoints for managing search Resultss.
    /// </summary>
    /// <param name="searchService"></param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Search Results")]
    [Route("api/v{version:apiVersion}/search-results")]
    public class SearchResultsController(ISearchResultsService searchService) : ControllerBase
    {
        /// <summary>
        /// Instance of Search Results service.
        /// </summary>
        private readonly ISearchResultsService _searchService = searchService;


        /// <summary>
        /// Creates a new search Results.
        /// </summary>
        /// <param name="inputModel">The search Results information.</param>
        /// <returns>The identifier of the newly created search Results.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the search Results cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<IEnumerable<int>>), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] SearchResultsInputModel model)
        {
            IEnumerable<int> result = await _searchService.CreateAsync(model);

            Response<IEnumerable<int>> response = new()
            {
                Code = StatusCodes.Status201Created,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }


    }
}
