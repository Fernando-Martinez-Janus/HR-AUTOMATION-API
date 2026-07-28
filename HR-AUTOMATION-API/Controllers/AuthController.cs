using Asp.Versioning;
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
    /// Provides endpoints for authenticating users.
    /// </summary>
    /// <param name="service">Instance of authentication service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Auth")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController(IAuthenticationService service) : ControllerBase
    {
        /// <summary>
        /// Instance of authentication service.
        /// </summary>
        private readonly IAuthenticationService _service = service;

        /// <summary>
        /// Authenticates a user using a Google ID token and issues the application's own JWT.
        /// </summary>
        /// <param name="model">The Google Sign-In request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The application access token and authenticated user information.</returns>
        /// <exception cref="ResponseExceptionFactory">
        /// Thrown when the Google token is missing or invalid, or the user does not exist or is inactive.
        /// </exception>
        [HttpPost("google")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<AuthenticationResponseViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Google([FromBody] GoogleLoginInputModel model, CancellationToken cancellationToken)
        {
            AuthenticationResponseViewModel result = await _service.LoginWithGoogleAsync(model, cancellationToken);

            Response<AuthenticationResponseViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Authenticates a user using an email and password and issues the application's own JWT.
        /// </summary>
        /// <param name="model">The email/password login request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The application access token and authenticated user information.</returns>
        /// <exception cref="ResponseExceptionFactory">
        /// Thrown when the email/password is missing or invalid, or the user is inactive.
        /// </exception>
        [HttpPost("login")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<AuthenticationResponseViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginInputModel model, CancellationToken cancellationToken)
        {
            AuthenticationResponseViewModel result = await _service.LoginWithEmailAsync(model, cancellationToken);

            Response<AuthenticationResponseViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }
    }
}
