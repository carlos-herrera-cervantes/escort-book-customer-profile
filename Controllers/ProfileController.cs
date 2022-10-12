using System.Threading.Tasks;
using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EscortBookCustomerProfile.Controllers;

[Route("api/v1/customer/profile")]
[ApiController]
public class ProfileController : ControllerBase
{
    #region snippet_Properties

    private readonly IProfileRepository _profileRepository;

    #endregion

    #region snippet_Constructors

    public ProfileController(IProfileRepository profileRepository)
        => _profileRepository = profileRepository;

    #endregion

    #region snippet_ActionMethods

    [HttpGet]
    public async Task<IActionResult> GetByIdAsync([FromHeader(Name = "user-id")] string userId)
    {
        var profile = await _profileRepository.GetByIdAsync(userId);

        if (profile is null) return NotFound();

        return Ok(profile);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateByIdAsync
    (
        [FromBody] UpdateProfileDTO profile,
        [FromHeader(Name = "user-id")] string userId
    )
    {
        var currentProfile = await _profileRepository.GetByIdAsync(userId);

        if (currentProfile is null) return NotFound();

        currentProfile.FirstName = profile.FirstName ?? currentProfile.FirstName;
        currentProfile.LastName = profile.LastName ?? currentProfile.LastName;
        currentProfile.Gender = profile.Gender ?? currentProfile.Gender;
        currentProfile.Birthdate = profile.Birthdate ?? currentProfile.Birthdate;

        await _profileRepository.UpdateByIdAsync(currentProfile);
        return Ok(currentProfile);
    }

    #endregion
}
