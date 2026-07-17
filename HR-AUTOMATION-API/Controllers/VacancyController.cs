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
    /// Provides endpoints for managing vacancies.
    /// </summary>
    /// <param name="vacancyService">Instance of Vacancy service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Vacancies")]
    [Route("api/v{version:apiVersion}/vacancies")]
    public class VacancyController(IVacancyService vacancyService) : ControllerBase
    {
        private readonly IVacancyService _vacancyService = vacancyService;

        /// <summary>
        /// Retrieves vacancies matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A paginated collection of matching vacancies.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<PaginationResponse<VacancyViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] VacancySearchInputModel model)
        {
            PaginationResponse<VacancyViewModel> result = await _vacancyService.SearchAsync(model);

            Response<PaginationResponse<VacancyViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves a vacancy by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the vacancy.</param>
        /// <returns>The requested <see cref="VacancyViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified vacancy does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<VacancyViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            VacancyViewModel result = await _vacancyService.GetAsync(id);

            Response<VacancyViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new vacancy.
        /// </summary>
        /// <param name="model">The vacancy information.</param>
        /// <returns>The identifier of the newly created vacancy.</returns>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] VacancyInputModel model)
        {
            int result = await _vacancyService.CreateAsync(model);

            Response<int> response = new()
            {
                Code = StatusCodes.Status201Created,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates or updates a vacancy (draft). If the vacancy identifier is provided, the existing vacancy is updated; otherwise, a new one is created.
        /// </summary>
        /// <param name="model">The vacancy information.</param>
        /// <returns>The updated <see cref="VacancyViewModel"/>.</returns>
        [HttpPost("draft")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<VacancyViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upsert([FromBody] VacancyInputModel model)
        {
            VacancyViewModel result = await _vacancyService.UpsertAsync(model);

            Response<VacancyViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return Ok(response);
        }

        /// <summary>
        /// Updates an existing vacancy.
        /// </summary>
        /// <param name="id">The identifier of the vacancy to update.</param>
        /// <param name="model">The updated vacancy information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] VacancyInputModel model)
        {
            await _vacancyService.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing vacancy.
        /// </summary>
        /// <param name="id">The identifier of the vacancy to delete.</param>
        [HttpDelete("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _vacancyService.DeleteAsync(id);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }
    }
}
