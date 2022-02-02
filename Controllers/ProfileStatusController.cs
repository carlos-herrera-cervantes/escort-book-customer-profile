using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Controllers
{
    [Route("api/v1/customer/profile/{profileId}/status")]
    public class ProfileStatusController : ControllerBase
    {
        #region snippet_Properties

        private readonly IProfileStatusRepository _profileStatusRepository;

        #endregion

        #region snippet_Constructors

        public ProfileStatusController(IProfileStatusRepository profileStatusRepository)
            => _profileStatusRepository = profileStatusRepository;

        #endregion

        #region snippet_ActionMethods

        [HttpPatch]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] string profileId, [FromBody] JsonPatchDocument<ProfileStatus> currentProfileStatus)
        {
            var profileStatus = await _profileStatusRepository.GetByIdAsync(profileId);

            if (profileStatus is null) return NotFound();

            await _profileStatusRepository.UpdateByIdAsync(profileStatus, currentProfileStatus);

            return Ok(profileStatus);
        }

        #endregion
    }
}
