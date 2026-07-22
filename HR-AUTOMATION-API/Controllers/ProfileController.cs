using Asp.Versioning;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.ViewModels;
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

    /// <summary>
    /// Retrieves all profiles for the specified or current organization.
    /// </summary>
    /// <param name="organizationId">Optional organization identifier.</param>
    /// <returns>A list of <see cref="ProfileViewModel"/>.</returns>
    [HttpGet]
    [MapToApiVersion("1")]
    [ProducesResponseType(typeof(Response<IEnumerable<ProfileViewModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int? organizationId = null)
    {
        IEnumerable<ProfileViewModel> result = await _profileService.GetAllAsync(organizationId);

        Response<IEnumerable<ProfileViewModel>> response = new()
        {
            Code = StatusCodes.Status200OK,
            DataResponse = result
        };

        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Retrieves a profile by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the profile.</param>
    /// <returns>The requested <see cref="ProfileViewModel"/>.</returns>
    /// <exception cref="ResponseExceptionFactory">Thrown when the specified profile does not exist.</exception>
    [HttpGet("{id:int}")]
    [MapToApiVersion("1")]
    [ProducesResponseType(typeof(Response<ProfileViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(int id)
    {
        // 💡 AQUÍ: Debes llamar a GetAsync(id) pasando el parámetro 'id'
        ProfileViewModel result = await _profileService.GetAsync(id);

        Response<ProfileViewModel> response = new()
        {
            Code = StatusCodes.Status200OK,
            DataResponse = result
        };

        return StatusCode(response.Code, response);
    }

}
