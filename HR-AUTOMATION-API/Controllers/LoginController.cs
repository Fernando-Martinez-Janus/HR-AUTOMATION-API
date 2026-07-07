using Asp.Versioning;
using HR_AUTOMATION.Application;
using HR_AUTOMATION.Application.InputModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HR_AUTOMATION_API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/auth/login")]
    [EnableRateLimiting("general")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginInputModel loginInputModel)
        {

            var token = await _loginService.ExecuteAsync(loginInputModel);
            return Ok(new { Token = token });
        }
    }
}
