using Asp.Versioning;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;



namespace HR_AUTOMATION_API.Controllers
{
    /// <summary>Handles HTTP requests for seniority level operations.</summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/skill-levels")]
    [EnableRateLimiting("general")]


    public class SkillLevelController : ControllerBase
    {
        private readonly ISkillLevelService _skillLevelService;

        /// <summary>Initializes a new instance of <see cref="SkillLevelController"/>.</summary>
        public SkillLevelController(ISkillLevelService skillLevelService)
        {
            _skillLevelService = skillLevelService;
        }


        /// <summary>Gets all skill levels for an organization.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int organizationId, [FromQuery] int rows_page, [FromQuery] int page_number)
        {
            IEnumerable<SkillLevelViewModel>? result = await _skillLevelService.GetAllAsync(organizationId, rows_page, page_number);

            Response<IEnumerable<SkillLevelViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result,
            };

            return StatusCode(response.Code, response);
        }





        /// <summary>Gets a skill level by its identifier.</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            SkillLevelViewModel? result = await _skillLevelService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }



        /// <summary>Creates a new skill level.</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SkillLevelInputModel skillLevelInputModel)
        {
            await _skillLevelService.CreateAsync(skillLevelInputModel);
            return NoContent();

        }


        /// <summary>Updates an existing skill level.</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SkillLevelInputModel skillLevelInputModel)
        {
            await _skillLevelService.UpdateAsync(id, skillLevelInputModel);
            return NoContent();
        }



        /// <summary>Soft-deletes (disables) a skill level.</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDelete(int id, [FromQuery] int updatedBy)
        {
            await _skillLevelService.SoftDeleteAsync(id, updatedBy);
            return NoContent();
        }
    }
}

