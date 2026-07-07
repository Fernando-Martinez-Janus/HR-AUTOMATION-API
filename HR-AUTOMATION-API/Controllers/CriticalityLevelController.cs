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
    [Route("api/v{version:apiVersion}/criticality-levels")]
    [EnableRateLimiting("general")]


    public class CriticalityLevelController : ControllerBase
    {
        private readonly ICriticalityLevelService _criticalityLevelService;

        /// <summary>Initializes a new instance of <see cref="CriticalityLevelController"/>.</summary>
        public CriticalityLevelController(ICriticalityLevelService criticalityLevelService)
        {
            _criticalityLevelService = criticalityLevelService;
        }


        /// <summary>Gets all criticality levels for an organization.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int organizationId, [FromQuery] int rows_page, [FromQuery] int page_number)
        {
            IEnumerable<CriticalityLevelViewModel>? result = await _criticalityLevelService.GetAllAsync(organizationId, rows_page, page_number);

            Response<IEnumerable<CriticalityLevelViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result,
            };

            return StatusCode(response.Code, response);
        }





        /// <summary>Gets a criticality level by its identifier.</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            CriticalityLevelViewModel? result = await _criticalityLevelService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }



        /// <summary>Creates a new criticality level.</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CriticalityLevelInputModel criticalityLevelInputModel)
        {
            await _criticalityLevelService.CreateAsync(criticalityLevelInputModel);
            return NoContent();

        }


        /// <summary>Updates an existing criticality level.</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CriticalityLevelInputModel criticalityLevelInputModel)
        {
            await _criticalityLevelService.UpdateAsync(id, criticalityLevelInputModel);
            return NoContent();
        }



        /// <summary>Soft-deletes (disables) a criticality level.</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDelete(int id, [FromQuery] int updatedBy)
        {
            await _criticalityLevelService.SoftDeleteAsync(id, updatedBy);
            return NoContent();
        }
    }
}

