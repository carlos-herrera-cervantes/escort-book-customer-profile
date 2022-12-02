using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using EscortBookCustomerProfile.Web.Attributes;
using EscortBookCustomerProfile.Web.Models;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Services;
using EscortBookCustomerProfile.Web.Constants;

namespace EscortBookCustomerProfile.Web.Controllers;

[Route("api/v1/customer")]
[ApiController]
public class IdentificationController : ControllerBase
{
    #region snippet_Properties

    private readonly IIdentificationRepository _identificationRepository;

    private readonly IAWSS3Service _s3Service;

    #endregion

    #region snippet_Constructors

    public IdentificationController(IIdentificationRepository identificationRepository, IAWSS3Service s3Service)
    {
        _identificationRepository = identificationRepository;
        _s3Service = s3Service;
    }

    #endregion

    #region snippet_ActionMethods

    [HttpGet("{id}/profile/identification")]
    public async Task<IActionResult> GetByExternalAsync([FromRoute] string id)
    {
        var rows = await _identificationRepository.GetAllAsync(i => i.ID == id);
        var identifications = rows.Select(r =>
        {
            r.Path = $"{S3.Endpoint}/{S3.BucketName}/{r.Path}";
            return r;
        });

        return Ok(identifications);
    }

    [HttpGet("profile/identification/{identificationPartID}")]
    public async Task<IActionResult> GetByIdAsync([FromHeader(Name = "user-id")] string userId, [FromRoute] string identificationPartID)
    {
        var identification = await _identificationRepository
            .GetAsync(i => i.CustomerID == userId && i.IdentificationPartID == identificationPartID);

        if (identification is null) return NotFound();

        identification.Path = $"{S3.Endpoint}/{S3.BucketName}/{identification.Path}";

        return Ok(identification);
    }

    [HttpPost("profile/identification")]
    [IdentificationPartExists]
    [RequestSizeLimit(2_000_000)]
    public async Task<IActionResult> CreateAsync([FromHeader(Name = "user-id")] string userId, [FromForm] CreateIdentification dto)
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

    [HttpPatch("profile/identification/{identificationPartID}")]
    [IdentificationPartExists]
    [RequestSizeLimit(2_000_000)]
    public async Task<IActionResult> UpdateByIdAsync(
        [FromHeader(Name = "user-id")] string userId,
        [FromRoute] string identificationPartID,
        [FromForm] IFormFile image
    )
    {
        var newIdentification = await _identificationRepository
            .GetAsync(i => i.CustomerID == userId && i.IdentificationPartID == identificationPartID);

        if (newIdentification is null) return NotFound();

        var imageStream = image.OpenReadStream();
        var url = await _s3Service.PutObjectAsync(image.FileName, userId, imageStream);

        var currentIdentification = new JsonPatchDocument<Identification>();
        currentIdentification.Replace(a => a.Path, $"{userId}/{image.FileName}");

        await _identificationRepository.UpdateAsync(newIdentification, currentIdentification);

        newIdentification.Path = url;

        return Ok(newIdentification);
    }

    #endregion
}
