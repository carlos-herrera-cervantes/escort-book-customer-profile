using EscortBookCustomerProfile.Attributes;
using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using EscortBookCustomerProfile.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Controllers
{
    [Route("api/v1/customer/profile/identification")]
    [ApiController]
    public class IdentificationController : ControllerBase
    {
        #region snippet_Properties

        private readonly IIdentificationRepository _identificationRepository;

        private readonly IAWSS3Service _s3Service;

        private readonly IConfiguration _configuration;

        #endregion

        #region snippet_Constructors

        public IdentificationController
        (
            IIdentificationRepository identificationRepository,
            IAWSS3Service s3Service,
            IConfiguration configuration
        )
        {
            _identificationRepository = identificationRepository;
            _s3Service = s3Service;
            _configuration = configuration;
        }

        #endregion

        #region snippet_ActionMethods

        [HttpGet("{identificationPartID}")]
        public async Task<IActionResult> GetByIdAsync
        (
            [FromHeader(Name = "user-id")] string userId,
            [FromRoute] string identificationPartID
        )
        {
            var identification = await _identificationRepository.GetByIdAsync(userId, identificationPartID);

            if (identification is null) return NotFound();

            var endpoint = _configuration["AWS:S3:Endpoint"];
            var bucketName = _configuration["AWS:S3:Name"];

            identification.Path = $"{endpoint}/{bucketName}/{identification.Path}";

            return Ok(identification);
        }

        [HttpPost]
        [IdentificationPartExists]
        [RequestSizeLimit(2_000_000)]
        public async Task<IActionResult> CreateAsync
        (
            [FromHeader(Name = "user-id")] string userId,
            [FromForm] CreateIdentificationDto dto
        )
        {
            var (image, identificationPartID) = dto;
            var imageStream = image.OpenReadStream();
            var url = await _s3Service.PutObjectAsync(image.FileName, userId, imageStream);

            var identification = new Identification();
            identification.CustomerID = userId;
            identification.Path = $"{userId}/{image.FileName}";
            identification.IdentificationPartID = identificationPartID;

            await _identificationRepository.CreateAsync(identification);

            identification.Path = url;

            return Created("", identification);
        }

        [HttpPatch("{identificationPartID}")]
        [IdentificationPartExists]
        [RequestSizeLimit(2_000_000)]
        public async Task<IActionResult> UpdateByIdAsync
        (
            [FromHeader(Name = "user-id")] string userId,
            [FromRoute] string identificationPartID,
            [FromForm] IFormFile image
        )
        {
            var newIdentification = await _identificationRepository.GetByIdAsync(userId, identificationPartID);

            if (newIdentification is null) return NotFound();

            var imageStream = image.OpenReadStream();
            var url = await _s3Service.PutObjectAsync(image.FileName, userId, imageStream);

            var currentIdentification = new JsonPatchDocument<Identification>();
            currentIdentification.Replace(a => a.Path, $"{userId}/{image.FileName}");

            await _identificationRepository.UpdateByIdAsync(newIdentification, currentIdentification);

            newIdentification.Path = url;

            return Ok(newIdentification);
        }

        #endregion
    }
}
