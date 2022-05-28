using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using EscortBookCustomerProfile.Common;
using EscortBookCustomerProfile.Handlers;
using EscortBookCustomerProfile.Types;
using EscortBookCustomerProfile.Constants;

namespace EscortBookCustomerProfile.Controllers
{
    [Route("api/v1/customer")]
    public class ProfileStatusController : ControllerBase
    {
        #region snippet_Properties

        private readonly IProfileStatusRepository _profileStatusRepository;

        private readonly IProfileStatusCategoryRepository _profileStatusCategoryRepository;

        private readonly IOperationHandler<BlockUserEvent> _operationHandler;

        #endregion

        #region snippet_Constructors

        public ProfileStatusController
        (
            IProfileStatusRepository profileStatusRepository,
            IProfileStatusCategoryRepository profileStatusCategoryRepository,
            IOperationHandler<BlockUserEvent> operationHandler
        )
        {
            _profileStatusRepository = profileStatusRepository;
            _profileStatusCategoryRepository = profileStatusCategoryRepository;
            _operationHandler = operationHandler;
        }

        #endregion

        #region snippet_ActionMethods

        [HttpGet("profile-status-categories")]
        public async Task<IActionResult> GetProfileStatusCategoriesAsync()
            => Ok(await _profileStatusCategoryRepository.GetAllAsync());

        [HttpGet("{id}/profile/status")]
        public async Task<IActionResult> GetByExternalAsync([FromRoute] string id)
        {
            var profileStatus = await _profileStatusRepository.GetByIdAsync(id);

            if (profileStatus is null) return NotFound();

            var category = await _profileStatusCategoryRepository.GetByIdAsync(profileStatus.ProfileStatusCategoryID);

            return Ok(category);
        }

        [HttpPatch("{id}/profile/status")]
        public async Task<IActionResult> UpdateByExternal
        (
            [FromRoute] string id,
            [FromBody] UpdateProfileStatusDTO profile
        )
        {
            var category = await _profileStatusCategoryRepository.GetByIdAsync(profile.ProfileStatusCategoryID);

            if (category is null) return NotFound();
            
            var profileStatus = await _profileStatusRepository.GetByIdAsync(id);

            if (profileStatus is null) return NotFound();

            profileStatus.ProfileStatusCategoryID = profile.ProfileStatusCategoryID;

            await _profileStatusRepository.UpdateByIdAsync(profileStatus);

            if (category.Name == ValidProfileStatus.Locked || category.Name == ValidProfileStatus.Active)
            {
                EmitMessage(id, category.Name);
            }

            return Ok(profileStatus);
        }

        [HttpPatch("profile/status")]
        public async Task<IActionResult> UpdateByIdAsync
        (
            [FromBody] UpdateProfileStatusDTO profile,
            [FromHeader(Name = "user-id")] string userId
        )
        {
            var category = await _profileStatusCategoryRepository.GetByIdAsync(profile.ProfileStatusCategoryID);

            if (category is null) return NotFound();
            
            var validStatus = new List<string> { ValidProfileStatus.Deactivated, ValidProfileStatus.Deleted };
            var validOperation = validStatus.FirstOrDefault(s => s == category.Name);

            if (validOperation is null) return BadRequest();

            var profileStatus = await _profileStatusRepository.GetByIdAsync(userId);

            if (profileStatus is null) return NotFound();

            profileStatus.ProfileStatusCategoryID = profile.ProfileStatusCategoryID;

            await _profileStatusRepository.UpdateByIdAsync(profileStatus);

            if (category.Name == ValidProfileStatus.Deactivated) EmitMessage(userId, category.Name);

            return Ok(profileStatus);
        }

        #endregion
        
        #region snippe_Helpers

        private void EmitMessage(string userId, string categoryName)
        {
            var blockUserEvent = new BlockUserEvent { UserId = userId, Status = categoryName };
            Emitter<BlockUserEvent>.EmitMessage(_operationHandler, blockUserEvent);
        }
        
        #endregion
    }
}
