using EscortBookCustomerProfile.Common;
using EscortBookCustomerProfile.Handlers;
using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using EscortBookCustomerProfile.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Controllers
{
    [Route("api/v1/customer")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        #region snippet_Properties

        private readonly IPhotoRepository _photoRepository;

        private readonly IAWSS3Service _s3Service;

        private readonly IConfiguration _configuration;

        private readonly IOperationHandler<string> _operationHandler;

        #endregion

        #region snippet_Constructors

        public PhotoController
        (
            IPhotoRepository photoRepository,
            IAWSS3Service s3Service,
            IConfiguration configuration,
            IOperationHandler<string> operationHandler
        )
        {
            _photoRepository = photoRepository;
            _s3Service = s3Service;
            _configuration = configuration;
            _operationHandler = operationHandler;
        }

        #endregion

        #region snippet_ActionMethods

        [HttpGet("{id}/profile/photos")]
        public async Task<IActionResult> GetByExternalAsync([FromRoute] string id, [FromQuery] Pagination pagination)
        {
            var (page, pageSize, _) = pagination;
            var rows = await _photoRepository.GetAllAsync(id, page, pageSize);

            var endpoint = _configuration["AWS:S3:Endpoint"];
            var bucketName = _configuration["AWS:S3:Name"];
            var photos = rows.Select(r =>
            {
                r.Path = $"{endpoint}/{bucketName}/{r.Path}";
                return r;
            });

            return Ok(photos);
        }

        [HttpGet("profile/photos")]
        public async Task<IActionResult> GetAllAsync
        (
            [FromHeader(Name = "user-id")] string userId,
            [FromQuery] Pagination pagination
        )
        {
            var (page, pageSize, _) = pagination;
            var rows = await _photoRepository.GetAllAsync(userId, page, pageSize);

            var endpoint = _configuration["AWS:S3:Endpoint"];
            var bucketName = _configuration["AWS:S3:Name"];
            var photos = rows.Select(r =>
            {
                r.Path = $"{endpoint}/{bucketName}/{r.Path}";
                return r;
            });

            return Ok(photos);
        }

        [HttpPost("profile/photos")]
        [RequestSizeLimit(2_000_000)]
        public async Task<IActionResult> CreateAsync
        (
            [FromHeader(Name = "user-id")] string userId,
            [Required][FromForm] IFormFile image
        )
        {
            var imageStream = image.OpenReadStream();
            var url = await _s3Service.PutObjectAsync(image.FileName, userId, imageStream);

            var photo = new Photo();
            photo.CustomerID = userId;
            photo.Path = $"{userId}/{image.FileName}";

            await _photoRepository.CreateAsync(photo);

            photo.Path = url;

            return Created("", photo);
        }

        [HttpDelete("profile/photos/{id}")]
        public async Task<IActionResult> DeleteByIdAsync
        (
            [FromHeader(Name = "user-id")] string userId,
            [FromRoute] string id
        )
        {
            var photo = await _photoRepository.GetByIdAsync(userId, id);

            if (photo is null) return NotFound();

            await _photoRepository.DeleteByIdAsync(photo.ID);
            Emitter<string>.EmitMessage(_operationHandler, photo.Path);

            return NoContent();
        }

        #endregion
    }
}
