using Asp.Versioning;
using HR_AUTOMATION.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;



namespace HR_AUTOMATION_API.Controllers
{
    /// <summary>Handles HTTP requests for seniority level operations.</summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/profiles")]
    [EnableRateLimiting("general")]


    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        /// <summary>Initializes a new instance of <see cref="ProfileController"/>.</summary>
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }


        /// <summary>Gets all area levels for an organization.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int organizationId, [FromQuery] int rows_page, [FromQuery] int page_number)
        {
            IEnumerable<ProfileViewModel>? result = await _profileService.GetAllAsync(organizationId, rows_page, page_number);

            Response<IEnumerable<ProfileViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result,
            };

            return StatusCode(response.Code, response);
        }





        /// <summary>Gets a area level by its identifier.</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, int organizationId)
        {
            ProfileViewModel? result = await _profileService.GetByIdAsync(id, organizationId);
            if (result == null) return NotFound();
            return Ok(result);
        }



        /// <summary>Creates a new area level.</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProfileInputModel profileInputModel)
        {
            await _profileService.CreateAsync(profileInputModel);
            return NoContent();

        }


        /// <summary>Updates an existing area level.</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProfileInputModel profileInputModel)
        {
            await _profileService.UpdateAsync(id, profileInputModel);
            return NoContent();
        }



        /// <summary>Soft-deletes (disables) a area level.</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDelete(int id, int organizationId, [FromQuery] int updatedBy)
        {
            await _profileService.SoftDeleteAsync(id, organizationId, updatedBy);
            return NoContent();
        }
    }
}

