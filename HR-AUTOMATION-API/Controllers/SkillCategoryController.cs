using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;

namespace HR_AUTOMATION_API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/skill-categories")]
    [EnableRateLimiting("general")]
    public class SkillCategoryController : ControllerBase
    {
        private readonly ISkillCategoryService _skillCategoryService;

        public SkillCategoryController(ISkillCategoryService skillCategoryService)
        {
            _skillCategoryService = skillCategoryService;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAll([FromQuery] int organizationId, [FromQuery] int rows_page, [FromQuery] int page_number)
        {
            var result = await _skillCategoryService.GetAllAsync(organizationId, rows_page, page_number);
            var response = new Response<IEnumerable<SkillCategoryViewModel>>
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
            var result = await _skillCategoryService.GetByIdAsync(id);
            if (result == null) return NotFound();
            var response = new Response<SkillCategoryViewModel>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return Ok(response);
        }

        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] SkillCategoryInputModel input)
        {
            var result = await _skillCategoryService.CreateAsync(input);
            var response = new Response<int>
            {
                Code = StatusCodes.Status201Created,
                DataResponse = result.Id
            };
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(int id, [FromBody] SkillCategoryInputModel input)
        {
            await _skillCategoryService.UpdateAsync(id, input);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int updatedBy)
        {
            await _skillCategoryService.DeleteAsync(id, updatedBy);
            return NoContent();
        }
    }
}
