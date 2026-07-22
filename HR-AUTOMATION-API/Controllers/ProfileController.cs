using Asp.Versioning;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Constants;

namespace HR_AUTOMATION_API.Controllers;

/// <summary>
/// Manages profile operations.
/// </summary>
/// <param name="profileService">Instance of Profile service.</param>
[ApiController]
[Produces(MediaTypes.Json)]
[EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
[Tags("Profiles")]
[Route("api/v{version:apiVersion}/profiles")]
public class ProfilesController(IProfileService profileService) : ControllerBase
{
    private readonly IProfileService _profileService = profileService;

    /// <summary>
    /// Creates a new profile with its associated skills.
    /// </summary>
    /// <param name="model">The profile information.</param>
    /// <returns>The identifier of the newly created profile.</returns>
    [HttpPost]
    [MapToApiVersion("1")]
    [ProducesResponseType(typeof(Response<int>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] ProfileInputModel model)
    {
        int result = await _profileService.CreateAsync(model);

        Response<int> response = new()
        {
            Code = StatusCodes.Status201Created,
            DataResponse = result
        };

        return StatusCode(response.Code, response);
    }
}
