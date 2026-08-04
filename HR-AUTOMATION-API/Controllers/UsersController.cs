using Asp.Versioning;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.ViewModels;
using HR_AUTOMATION.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Constants;

namespace HR_AUTOMATION_API.Controllers
{
    /// <summary>
    /// Provides endpoints for retrieving user profile information.
    /// </summary>
    /// <param name="service">Instance of user profile service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Users")]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController(IUserProfileService service) : ControllerBase
    {
        /// <summary>
        /// Instance of user profile service.
        /// </summary>
        private readonly IUserProfileService _service = service;

        /// <summary>
        /// Retrieves the profile of the currently authenticated user. The user identifier is
        /// obtained exclusively from the JWT claims of the request; it can never be supplied by
        /// the client, so one user can never retrieve another user's data through this endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The authenticated user's profile.</returns>
        [Authorize]
        [HttpGet("account")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<UserProfileViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAccount(CancellationToken cancellationToken)
        {
            UserProfileViewModel result = await _service.GetMyProfileAsync(cancellationToken);

            Response<UserProfileViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves another user's profile by identifier. Restricted to administrators.
        /// </summary>
        /// <param name="id">The identifier of the user to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The requested user's profile.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<UserProfileViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            UserProfileViewModel result = await _service.GetByIdAsync(id, cancellationToken);

            Response<UserProfileViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }
    }
}
