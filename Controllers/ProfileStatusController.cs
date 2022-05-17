using EscortBookCustomerProfile.Attributes;
using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Controllers
{
    [Route("api/v1/customer")]
    public class ProfileStatusController : ControllerBase
    {
        #region snippet_Properties

        private readonly IProfileStatusRepository _profileStatusRepository;

        private readonly IProfileStatusCategoryRepository _profileStatusCategoryRepository;

        #endregion

        #region snippet_Constructors

        public ProfileStatusController
        (
            IProfileStatusRepository profileStatusRepository,
            IProfileStatusCategoryRepository profileStatusCategoryRepository
        )
        {
            _profileStatusRepository = profileStatusRepository;
            _profileStatusCategoryRepository = profileStatusCategoryRepository;
        }

        #endregion

        #region snippet_ActionMethods

        [HttpGet("{id}/profile/status")]
        public async Task<IActionResult> GetByExternalAsync([FromRoute] string id)
        {
            var profileStatus = await _profileStatusRepository.GetByIdAsync(id);

            if (profileStatus is null) return NotFound();

            var category = await _profileStatusCategoryRepository.GetByIdAsync(profileStatus.ProfileStatusCategoryID);

            return Ok(category);
        }

        [HttpPatch("profile/status")]
        [ProfileStatusCategoryExists]
        public async Task<IActionResult> UpdateByIdAsync
        (
            [FromBody] UpdateProfileStatusDTO profile,
            [FromHeader(Name = "user-id")] string userId
        )
        {
            var profileStatus = await _profileStatusRepository.GetByIdAsync(userId);

            if (profileStatus is null) return NotFound();

            profileStatus.ProfileStatusCategoryID = profile.ProfileStatusCategoryID;

            await _profileStatusRepository.UpdateByIdAsync(profileStatus);

            return Ok(profileStatus);
        }

        #endregion
    }
}
