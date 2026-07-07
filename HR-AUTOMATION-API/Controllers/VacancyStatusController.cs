using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;

namespace HR_AUTOMATION_API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/vacancy-statuses")]
    [EnableRateLimiting("general")]
    public class VacancyStatusController : ControllerBase
    {
        private readonly IVacancyStatusService _vacancyStatusService;

        public VacancyStatusController(IVacancyStatusService vacancyStatusService)
        {
            _vacancyStatusService = vacancyStatusService;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAll([FromQuery] int organizationId, [FromQuery] int rows_page, [FromQuery] int page_number)
        {
            var result = await _vacancyStatusService.GetAllAsync(organizationId, rows_page, page_number);
            var response = new Response<IEnumerable<VacancyStatusViewModel>>
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
            var result = await _vacancyStatusService.GetByIdAsync(id);
            if (result == null) return NotFound();
            var response = new Response<VacancyStatusViewModel>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return Ok(response);
        }

        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] VacancyStatusInputModel input)
        {
            var result = await _vacancyStatusService.CreateAsync(input);
            var response = new Response<int>
            {
                Code = StatusCodes.Status201Created,
                DataResponse = result.Id
            };
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(int id, [FromBody] VacancyStatusInputModel input)
        {
            await _vacancyStatusService.UpdateAsync(id, input);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int updatedBy)
        {
            await _vacancyStatusService.DeleteAsync(id, updatedBy);
            return NoContent();
        }
    }
}
