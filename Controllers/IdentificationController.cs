﻿using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using EscortBookCustomerProfile.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Controllers
{
    [Route("api/v1/profiles/{profileId}/identification")]
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
        public async Task<IActionResult> GetByIdAsync([FromRoute] string profileId, [FromRoute] string identificationPartID)
        {
            var identification = await _identificationRepository.GetByIdAsync(profileId, identificationPartID);

            if (identification is null) return NotFound();

            var endpoint = _configuration["AWS:S3:Endpoint"];
            var bucketName = _configuration["AWS:S3:Name"];

            identification.Path = $"{endpoint}/{bucketName}/{identification.Path}";

            return Ok(identification);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromRoute] string profileId, [FromForm] CreateIdentificationDto dto)
        {
            var (image, identificationPartID) = dto;
            var imageStream = image.OpenReadStream();
            var url = await _s3Service.PutObjectAsync(image.FileName, profileId, imageStream);

            var identification = new Identification();
            identification.ProfileID = profileId;
            identification.Path = $"{profileId}/{image.FileName}";
            identification.IdentificationPartID = identificationPartID;

            await _identificationRepository.CreateAsync(identification);

            identification.Path = url;

            return Created("", identification);
        }

        [HttpPatch("{identificationPartID}")]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] string profileId, [FromRoute] string identificationPartID, [FromForm] IFormFile image)
        {
            var newIdentification = await _identificationRepository.GetByIdAsync(profileId, identificationPartID);

            if (newIdentification is null) return NotFound();

            var imageStream = image.OpenReadStream();
            var url = await _s3Service.PutObjectAsync(image.FileName, profileId, imageStream);

            var currentIdentification = new JsonPatchDocument<Identification>();
            currentIdentification.Replace(a => a.Path, $"{profileId}/{image.FileName}");

            await _identificationRepository.UpdateByIdAsync(newIdentification, currentIdentification);

            newIdentification.Path = url;

            return Ok(newIdentification);
        }

        #endregion
    }
}
