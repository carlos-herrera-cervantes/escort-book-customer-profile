using System.Threading.Tasks;
using EscortBookCustomerProfile.Common;
using EscortBookCustomerProfile.Handlers;
using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EscortBookCustomerProfile.Controllers
{
    [Route("api/v1/profiles")]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var profile = await _profileRepository.GetByIdAsync(id);
            
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] string id, JsonPatchDocument<Profile> currentProfile)
        {
            var newProfile = await _profileRepository.GetByIdAsync(id);

            if (newProfile is null) return NotFound();

            await _profileRepository.UpdateByIdAsync(newProfile, currentProfile);
            return Ok(newProfile);
        }

        #endregion
    }
}