using Asp.Versioning;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;

namespace HR_AUTOMATION_API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/roles")]
    [EnableRateLimiting("general")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAll([FromQuery] int organizationId, [FromQuery] int rows_page, [FromQuery] int page_number)
        {
            var result = await _roleService.GetAllRolesAsync(organizationId, rows_page, page_number);
            var response = new Response<IEnumerable<RoleViewModel>>
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
            var result = await _roleService.GetByIdAsync(id);
            if (result == null) return NotFound();
            var response = new Response<RoleViewModel>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return Ok(response);
        }

        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] RoleInputModel input)
        {
            var result = await _roleService.CreateRoleAsync(input);
            var response = new Response<long>
            {
                Code = StatusCodes.Status201Created,
                DataResponse = result.Id
            };
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPut("{id:long}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(long id, [FromBody] RoleInputModel input)
        {
            await _roleService.UpdateRoleAsync(id, input);
            return NoContent();
        }

        [HttpPatch("{id:long}/permissions")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateRolePermissions(long id, [FromBody] RolePermissionInputModel input)
        {
            var result = await _roleService.UpdateRolePermissionAsync(id, input.PermissionIds);
            var response = new Response<RoleViewModel>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return StatusCode(response.Code, response);
        }

        [HttpDelete("{id:long}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Delete(long id, [FromQuery] int updatedBy)
        {
            await _roleService.DeleteRoleAsync(id, updatedBy);
            return NoContent();
        }
    }
}
