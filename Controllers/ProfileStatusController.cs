using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Controllers
{
    [Route("api/v1/customer/profile/status")]
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
        public async Task<IActionResult> UpdateByIdAsync([FromBody] UpdateProfileStatusDTO profile)
        {
            var profileStatus = await _profileStatusRepository.GetByIdAsync(profile.User.Id);

            if (profileStatus is null) return NotFound();

            profileStatus.ProfileStatusCategoryID = profile.ProfileStatusCategoryID;

            await _profileStatusRepository.UpdateByIdAsync(profileStatus);

            return Ok(profileStatus);
        }

        #endregion
    }
}
