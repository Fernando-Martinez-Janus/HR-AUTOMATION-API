using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;

namespace HR_AUTOMATION_API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/profile-skills")]
    [EnableRateLimiting("general")]
    public class ProfileSkillController : ControllerBase
    {
        private readonly IProfileSkillService _profileSkillService;

        public ProfileSkillController(IProfileSkillService profileSkillService)
        {
            _profileSkillService = profileSkillService;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAll([FromQuery] int organizationId, [FromQuery] int rows_page, [FromQuery] int page_number)
        {
            var result = await _profileSkillService.GetAllAsync(organizationId, rows_page, page_number);
            var response = new Response<IEnumerable<ProfileSkillViewModel>>
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
            var result = await _profileSkillService.GetByIdAsync(id);
            if (result == null) return NotFound();
            var response = new Response<ProfileSkillViewModel>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return Ok(response);
        }

        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] ProfileSkillInputModel input)
        {
            var result = await _profileSkillService.CreateAsync(input);
            var response = new Response<int>
            {
                Code = StatusCodes.Status201Created,
                DataResponse = result.Id
            };
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(int id, [FromBody] ProfileSkillInputModel input)
        {
            await _profileSkillService.UpdateAsync(id, input);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int updatedBy)
        {
            await _profileSkillService.DeleteAsync(id, updatedBy);
            return NoContent();
        }
    }
}
