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
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Vacancies")]
    [Route("api/v{version:apiVersion}/vacancies")]
    public class VacancyController(IVacancyService vacancyService) : ControllerBase
    {
        private readonly IVacancyService _vacancyService = vacancyService;

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
