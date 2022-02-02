using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using EscortBookCustomerProfile.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Controllers
{
    [Route("api/v1/customer/profile/{profileId}/photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        #region snippet_Properties

        private readonly IPhotoRepository _photoRepository;

        private readonly IAWSS3Service _s3Service;

        private readonly IConfiguration _configuration;

        #endregion

        #region snippet_Constructors

        public PhotoController
        (
            IPhotoRepository photoRepository,
            IAWSS3Service s3Service,
            IConfiguration configuration
        )
        {
            _photoRepository = photoRepository;
            _s3Service = s3Service;
            _configuration = configuration;
        }

        #endregion

        #region snippet_ActionMethods

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromRoute] string profileId, [FromQuery] Pagination pagination)
        {
            var (page, pageSize) = pagination;
            var rows = await _photoRepository.GetAllAsync(profileId, page, pageSize);

            var endpoint = _configuration["AWS:S3:Endpoint"];
            var bucketName = _configuration["AWS:S3:Name"];
            var photos = rows.Select(r =>
            {
                r.Path = $"{endpoint}/{bucketName}/{r.Path}";
                return r;
            });

            return Ok(photos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string profileId, [FromRoute] string id)
        {
            var photo = await _photoRepository.GetByIdAsync(profileId, id);

            if (photo is null) return NotFound();

            var endpoint = _configuration["AWS:S3:Endpoint"];
            var bucketName = _configuration["AWS:S3:Name"];

            photo.Path = $"{endpoint}/{bucketName}/{photo.Path}";

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromRoute] string profileId, [FromForm] IFormFile image)
        {
            var imageStream = image.OpenReadStream();
            var url = await _s3Service.PutObjectAsync(image.FileName, profileId, imageStream);

            var photo = new Photo();
            photo.ProfileID = profileId;
            photo.Path = $"{profileId}/{image.FileName}";

            await _photoRepository.CreateAsync(photo);

            photo.Path = url;

            return Created("", photo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] string profileId, [FromRoute] string id)
        {
            var photo = await _photoRepository.GetByIdAsync(profileId, id);

            if (photo is null) return NotFound();

            await _photoRepository.DeleteByIdAsync(photo.ID);

            return NoContent();
        }

        #endregion
    }
}
