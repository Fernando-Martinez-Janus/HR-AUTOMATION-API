using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.ViewModels;
using HR_AUTOMATION.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Constants;

namespace HR_AUTOMATION_API.Controllers
{
    /// <summary>
    /// Exposes authentication-related HTTP endpoints such as user login and token refresh.
    /// </summary>
    /// <param name="authService">Instance of Auth Service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Auth")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// Instance of Auth Service.
        /// </summary>
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Authenticates a user with the provided login credentials.
        /// </summary>
        /// <param name="model">The login input model containing username and password.</param>
        /// <returns>Generated authentication token and refresh token.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(Response<AuthViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginInputModel model)
        {
            AuthViewModel result = await _authService.LoginAsync(model);

            Response<AuthViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Refreshes the authentication token for the current user.
        /// </summary>
        /// <param name="model">Current refresh token.</param>
        /// <returns>Generated authentication token and refresh token.</returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(Response<AuthViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenInputModel model)
        {
            AuthViewModel result = await _authService.RefreshTokenAsync(model);

            Response<AuthViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }
    }
}