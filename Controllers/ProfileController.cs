using System.Threading.Tasks;
using EscortBookCustomerProfile.Common;
using EscortBookCustomerProfile.Handlers;
using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using EscortBookCustomerProfile.Types;
using Microsoft.AspNetCore.Mvc;

namespace EscortBookCustomerProfile.Controllers
{
    [Route("api/v1/customer/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        #region snippet_Properties

        private readonly IProfileRepository _profileRepository;

        private readonly IOperationHandler<Profile> _operationHandler;

        #endregion

        #region snippet_Constructors

        public ProfileController(IProfileRepository profileRepository, IOperationHandler<Profile> operationHandler)
        {
            _profileRepository = profileRepository;
            _operationHandler = operationHandler;
        }

        #endregion

        #region snippet_ActionMethods

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync([FromBody] Payload payload)
        {
            var profile = await _profileRepository.GetByIdAsync(payload.User.Id);
            
            if (profile is null) return NotFound();

            return Ok(profile);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Profile profile)
        {
            await _profileRepository.CreateAsync(profile);

            Emitter<Profile>.EmitMessage(_operationHandler, profile);

            return Created("", profile);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateByIdAsync([FromBody] UpdateProfileDTO profile)
        {
            var currentProfile = await _profileRepository.GetByIdAsync(profile.User.Id);

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
}