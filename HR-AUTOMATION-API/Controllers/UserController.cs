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
    [Route("api/v{version:apiVersion}/users")]
    [EnableRateLimiting("general")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Create([FromBody] UserInputModel input)
        {
            var result = await _userService.CreateUserAsync(input);
            var response = new Response<UserViewModel>
            {
                Code = StatusCodes.Status201Created,
                DataResponse = result
            };
            return StatusCode(response.Code, response);
        }

        [HttpGet("{id:long}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _userService.GetByIdAsync(id);
            var response = new Response<UserViewModel>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return StatusCode(response.Code, response);
        }

        [HttpPut("{id:long}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Update(long id, [FromBody] UserInputModel input)
        {
            var result = await _userService.UpdateUserAsync(id, input);
            var response = new Response<UserViewModel>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return StatusCode(response.Code, response);
        }

        [HttpPatch("{id:long}/password")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdatePassword(long id, [FromBody] string password, [FromQuery] int updatedBy)
        {
            await _userService.UpdatePasswordAsync(id, password, updatedBy);
            return NoContent();
        }

        [HttpPatch("{id:long}/roles")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateRoles(long id, [FromBody] UserRoleInputModel input)
        {
            var result = await _userService.UpdateUserRolesAsync(id, input.RolesIds);
            var response = new Response<UserViewModel>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return StatusCode(response.Code, response);
        }

        [HttpDelete("{id:long}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> SoftDelete(long id, [FromQuery] int updatedBy)
        {
            await _userService.SoftDeleteUserAsync(id, updatedBy);
            return NoContent();
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetAll([FromQuery] int organizationId, [FromQuery] int rows_page, [FromQuery] int page_number)
        {
            var result = await _userService.GetAllUsersAsync(organizationId, rows_page, page_number);
            var response = new Response<IEnumerable<UserViewModel>>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return StatusCode(response.Code, response);
        }
    }
}
