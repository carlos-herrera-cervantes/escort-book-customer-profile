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
    [Route("api/v1/customer")]
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

        [HttpGet("{id}/profile/avatar")]
        public async Task<IActionResult> GetByExternalAsync([FromRoute] string id)
        {
            var avatar = await _avatarRepository.GetByIdAsync(id);

            if (avatar is null) return NotFound();

            var endpoint = _configuration["AWS:S3:Endpoint"];
            var bucketName = _configuration["AWS:S3:Name"];
            
            avatar.Path = $"{endpoint}/{bucketName}/{avatar.Path}";

            return Ok(avatar);
        }

        [HttpGet("profile/avatar")]
        public async Task<IActionResult> GetByIdAsync([FromHeader(Name = "user-id")] string userId)
        {
            var avatar = await _avatarRepository.GetByIdAsync(userId);

            if (avatar is null) return NotFound();

            var endpoint = _configuration["AWS:S3:Endpoint"];
            var bucketName = _configuration["AWS:S3:Name"];
            
            avatar.Path = $"{endpoint}/{bucketName}/{avatar.Path}";

            return Ok(avatar);
        }

        [HttpPost("profile/avatar")]
        [RequestSizeLimit(2_000_000)]
        public async Task<IActionResult> CreateAsync([FromForm] IFormFile image, [FromHeader(Name = "user-id")] string userId)
        {
            var imageStream = image.OpenReadStream();
            var url = await _s3Service.PutObjectAsync(image.FileName, userId, imageStream);

            var avatar = new Avatar();
            avatar.CustomerID = userId;
            avatar.Path = $"{userId}/{image.FileName}";

            await _avatarRepository.CreateAsync(avatar);

            avatar.Path = url;

            return Created("", avatar);
        }

        [HttpPatch("profile/avatar")]
        [RequestSizeLimit(2_000_000)]
        public async Task<IActionResult> UpdateByIdAsync([FromForm] IFormFile image, [FromHeader(Name = "user-id")] string userId)
        {
            var newAvatar = await _avatarRepository.GetByIdAsync(userId);

            if (newAvatar is null) return NotFound();

            var imageStream = image.OpenReadStream();
            var url = await _s3Service.PutObjectAsync(image.FileName, userId, imageStream);

            var currentAvatar = new JsonPatchDocument<Avatar>();
            currentAvatar.Replace(a => a.Path, $"{userId}/{image.FileName}");

            await _avatarRepository.UpdateByIdAsync(newAvatar, currentAvatar);

            newAvatar.Path = url;

            return Ok(newAvatar);
        }

        [HttpDelete("profile/avatar")]
        public async Task<IActionResult> DeleteByIdAsync([FromHeader(Name = "user-id")] string userId)
        {
            var avatar = await _avatarRepository.GetByIdAsync(userId);

            if (avatar is null) return NotFound();

            await _avatarRepository.DeleteByIdAsync(avatar.ID);

            return NoContent();
        }

        #endregion
    }
}