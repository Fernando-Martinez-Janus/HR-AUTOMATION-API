using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;

namespace HR_AUTOMATION_API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/vacancies")]
    [EnableRateLimiting("general")]
    public class VacancyController : ControllerBase
    {
        private readonly IVacancyService _vacancyService;

        public VacancyController(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAll([FromQuery] int organizationId, [FromQuery] int rows_page, [FromQuery] int page_number)
        {
            var result = await _vacancyService.GetAllAsync(organizationId, rows_page, page_number);
            var response = new Response<IEnumerable<VacancyViewModel>>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _vacancyService.GetByIdAsync(id);
            if (result == null) return NotFound();
            var response = new Response<VacancyViewModel>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return Ok(response);
        }

        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] VacancyInputModel input)
        {
            var result = await _vacancyService.CreateAsync(input);
            var response = new Response<int>
            {
                Code = StatusCodes.Status201Created,
                DataResponse = result.Id
            };
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost("draft")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Upsert([FromBody] VacancyInputModel input)
        {
            var result = await _vacancyService.UpsertAsync(input);
            var response = new Response<VacancyViewModel>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(int id, [FromBody] VacancyInputModel input)
        {
            await _vacancyService.UpdateAsync(id, input);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int updatedBy)
        {
            await _vacancyService.DeleteAsync(id, updatedBy);
            return NoContent();
        }
    }
}
