using System.Threading.Tasks;
using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using EscortBookCustomerProfile.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EscortBookCustomerProfile.Controllers
{
    [Route("api/v1/customer/profile/{profileId}/avatar")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        #region snippet_Properties

        private readonly IAvatarRepository _avatarRepository;

        private readonly IAWSS3Service _s3Service;

        private readonly IConfiguration _configuration;

        #endregion

        #region snippet_Constructors

        public AvatarController
        (
            IAvatarRepository avatarRepository,
            IAWSS3Service s3Service,
            IConfiguration configuration
        )
        {
            _avatarRepository = avatarRepository;
            _s3Service = s3Service;
            _configuration = configuration;
        }

        #endregion

        #region snippet_ActionMethods

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string profileId)
        {
            var avatar = await _avatarRepository.GetByIdAsync(profileId);

            if (avatar is null) return NotFound();

            var endpoint = _configuration["AWS:S3:Endpoint"];
            var bucketName = _configuration["AWS:S3:Name"];
            
            avatar.Path = $"{endpoint}/{bucketName}/{avatar.Path}";

            return Ok(avatar);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromRoute] string profileId, [FromForm] IFormFile image)
        {
            var imageStream = image.OpenReadStream();
            var url = await _s3Service.PutObjectAsync(image.FileName, profileId, imageStream);

            var avatar = new Avatar();
            avatar.ProfileID = profileId;
            avatar.Path = $"{profileId}/{image.FileName}";

            await _avatarRepository.CreateAsync(avatar);

            avatar.Path = url;

            return Created("", avatar);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] string profileId, [FromForm] IFormFile image)
        {
            var newAvatar = await _avatarRepository.GetByIdAsync(profileId);

            if (newAvatar is null) return NotFound();

            var imageStream = image.OpenReadStream();
            var url = await _s3Service.PutObjectAsync(image.FileName, profileId, imageStream);

            var currentAvatar = new JsonPatchDocument<Avatar>();
            currentAvatar.Replace(a => a.Path, $"{profileId}/{image.FileName}");

            await _avatarRepository.UpdateByIdAsync(newAvatar, currentAvatar);

            newAvatar.Path = url;

            return Ok(newAvatar);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] string profileId)
        {
            var avatar = await _avatarRepository.GetByIdAsync(profileId);

            if (avatar is null) return NotFound();

            await _avatarRepository.DeleteByIdAsync(avatar.ID);

            return NoContent();
        }

        #endregion

    }
}