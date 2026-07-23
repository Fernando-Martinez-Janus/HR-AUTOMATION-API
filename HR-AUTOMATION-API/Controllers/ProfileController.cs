using Asp.Versioning;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.ViewModels;
using HR_AUTOMATION.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Constants;
using Shared.Kernel.ViewModels;

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
    /// Retrieves profiles matching the specified search criteria.
    /// </summary>
    /// <param name="model">The search criteria.</param>
    /// <returns>A collection of matching profiles.</returns>
    [HttpGet]
    [MapToApiVersion("1")]
    [ProducesResponseType(typeof(Response<PaginationResponse<ProfileViewModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] ProfileSearchInputModel model)
    {
        PaginationResponse<ProfileViewModel> result = await _profileService.SearchAsync(model);

        Response<PaginationResponse<ProfileViewModel>> response = new()
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
        ProfileViewModel result = await _profileService.GetAsync(id);

        Response<ProfileViewModel> response = new()
        {
            Code = StatusCodes.Status200OK,
            DataResponse = result
        };

        return StatusCode(response.Code, response);
    }


    /// <summary>
    /// Updates an existing profile.
    /// </summary>
    /// <param name="id">The identifier of the profile to update.</param>
    /// <param name="model">The updated profile information.</param>
    [HttpPut("{id:int}")]
    [MapToApiVersion("1")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(int id, [FromBody] ProfileInputModel model)
    {
        await _profileService.UpdateAsync(id, model);

        Response response = new()
        {
            Code = StatusCodes.Status204NoContent
        };

        return StatusCode(response.Code, response);
    }


    /// <summary>
    /// Deletes an existing profile.
    /// </summary>
    /// <param name="id">The identifier of the profile to delete.</param>
    [HttpDelete("{id:int}")]
    [MapToApiVersion("1")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await _profileService.DeleteAsync(id);

        Response response = new()
        {
            Code = StatusCodes.Status204NoContent
        };

        return StatusCode(response.Code, response);
    }
}