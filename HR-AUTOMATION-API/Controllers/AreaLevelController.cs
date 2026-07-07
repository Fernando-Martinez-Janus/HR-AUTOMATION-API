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
    [Route("api/v{version:apiVersion}/area-levels")]
    [EnableRateLimiting("general")]


    public class AreaLevelController : ControllerBase
    {
        private readonly IAreaLevelService _areaLevelService;

        /// <summary>Initializes a new instance of <see cref="AreaLevelController"/>.</summary>
        public AreaLevelController(IAreaLevelService areaLevelService)
        {
            _areaLevelService = areaLevelService;
        }


        /// <summary>Gets all area levels for an organization.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int organizationId, [FromQuery] int rows_page, [FromQuery] int page_number)
        {
            IEnumerable<AreaLevelViewModel>? result = await _areaLevelService.GetAllAsync(organizationId, rows_page, page_number);

            Response<IEnumerable<AreaLevelViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result,
            };

            return StatusCode(response.Code, response);
        }





        /// <summary>Gets a area level by its identifier.</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            AreaLevelViewModel? result = await _areaLevelService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }



        /// <summary>Creates a new area level.</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AreaLevelInputModel areaLevelInputModel)
        {
            await _areaLevelService.CreateAsync(areaLevelInputModel);
            return NoContent();

        }


        /// <summary>Updates an existing area level.</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AreaLevelInputModel areaLevelInputModel)
        {
            await _areaLevelService.UpdateAsync(id, areaLevelInputModel);
            return NoContent();
        }



        /// <summary>Soft-deletes (disables) a area level.</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDelete(int id, [FromQuery] int updatedBy)
        {
            await _areaLevelService.SoftDeleteAsync(id, updatedBy);
            return NoContent();
        }
    }
}

