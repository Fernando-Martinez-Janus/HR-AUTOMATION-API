using Asp.Versioning;
using HR_AUTOMATION.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;

namespace HR_AUTOMATION_API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/permissions")]
    [EnableRateLimiting("general")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAll([FromQuery] int organizationId, [FromQuery] int rows_page, [FromQuery] int page_number)
        {
            var result = await _permissionService.GetAllPermissionsAsync(organizationId, rows_page, page_number);
            var response = new Response<IEnumerable<PermissionViewModel>>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return Ok(response);
        }

        [HttpGet("{id:long}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _permissionService.GetByIdAsync(id);
            if (result == null) return NotFound();
            var response = new Response<PermissionViewModel>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return Ok(response);
        }

        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] PermissionInputModel input)
        {
            var result = await _permissionService.CreatePermissionAsync(input);
            var response = new Response<long>
            {
                Code = StatusCodes.Status201Created,
                DataResponse = result.Id
            };
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPut("{id:long}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(long id, [FromBody] PermissionInputModel input)
        {
            await _permissionService.UpdatePermissionAsync(id, input);
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(long id, [FromQuery] int updatedBy)
        {
            await _permissionService.DeletePermissionAsync(id, updatedBy);
            return NoContent();
        }
    }
}
