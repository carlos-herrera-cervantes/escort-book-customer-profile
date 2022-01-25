using System.Threading.Tasks;
using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EscortBookCustomerProfile.Controllers
{
    [Route("api/v1/{profileId}/avatar")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        #region snippet_Properties

        private readonly IAvatarRepository _avatarRepository;

        #endregion

        #region snippet_Constructors

        public AvatarController(IAvatarRepository avatarRepository) => _avatarRepository = avatarRepository;

        #endregion

        #region snippet_ActionMethods

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromRoute] string profileId)
        {
            var avatar = await _avatarRepository.GetByIdAsync(profileId);

            if (avatar is null) return NotFound();

            return Ok(avatar);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromRoute] string profileId, [FromBody] Avatar avatar)
        {
            avatar.ProfileID = profileId;
            await _avatarRepository.CreateAsync(avatar);
            return Created("", avatar);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateAsync([FromRoute] string profileId, JsonPatchDocument<Avatar> currentAvatar)
        {
            var newAvatar = await _avatarRepository.GetByIdAsync(profileId);

            if (newAvatar is null) return NotFound();

            await _avatarRepository.UpdateByIdAsync(newAvatar, currentAvatar);
            return Ok(newAvatar);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromRoute] string profileId)
        {
            var avatar = await _avatarRepository.GetByIdAsync(profileId);

            if (avatar is null) return NotFound();

            return NoContent();
        }

        #endregion

    }
}