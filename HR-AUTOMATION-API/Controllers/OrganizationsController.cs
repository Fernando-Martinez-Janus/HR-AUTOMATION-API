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
    /// Provides endpoints for managing organizations.
    /// </summary>
    /// <param name="service">Instance of Organization service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Organizations")]
    [Route("api/v{version:apiVersion}/organizations")]
    public class OrganizationsController(IOrganizationService service) : ControllerBase
    {
        /// <summary>
        /// Instance of Organization service.
        /// </summary>
        private readonly IOrganizationService _service = service;

        /// <summary>
        /// Retrieves organizations matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A paginated collection of matching organizations.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<PaginationResponse<OrganizationViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] OrganizationSearchInputModel model)
        {
            PaginationResponse<OrganizationViewModel> result = await _service.SearchAsync(model);

            Response<PaginationResponse<OrganizationViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves an organization by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the organization.</param>
        /// <returns>The requested <see cref="OrganizationViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified organization does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<OrganizationViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            OrganizationViewModel result = await _service.GetAsync(id);

            Response<OrganizationViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new organization.
        /// </summary>
        /// <param name="model">The organization information.</param>
        /// <returns>The identifier of the newly created organization.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the organization cannot be created.</exception>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] OrganizationInputModel model)
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
        /// Updates an existing organization.
        /// </summary>
        /// <param name="id">The identifier of the organization to update.</param>
        /// <param name="model">The updated organization information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] OrganizationInputModel model)
        {
            await _service.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing organization.
        /// </summary>
        /// <param name="id">The identifier of the organization to delete.</param>
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
