using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using EscortBookCustomerProfile.Web.Common;
using EscortBookCustomerProfile.Web.Handlers;
using EscortBookCustomerProfile.Web.Models;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Services;
using EscortBookCustomerProfile.Web.Constants;

namespace EscortBookCustomerProfile.Web.Controllers;

[Route("api/v1/customer")]
[ApiController]
public class PhotoController : ControllerBase
{
    #region snippet_Properties

    private readonly IPhotoRepository _photoRepository;

    private readonly IAWSS3Service _s3Service;

    private readonly IOperationHandler<string> _operationHandler;

    #endregion

    #region snippet_Constructors

    public PhotoController(IPhotoRepository photoRepository, IAWSS3Service s3Service, IOperationHandler<string> operationHandler)
    {
        _photoRepository = photoRepository;
        _s3Service = s3Service;
        _operationHandler = operationHandler;
    }

    #endregion

    #region snippet_ActionMethods

    [HttpGet("{id}/profile/photos")]
    [ExcludeFromCodeCoverage]
    public async Task<IActionResult> GetByExternalAsync([FromRoute] string id, [FromQuery] Pagination pagination)
    {
        var (page, pageSize, _) = pagination;
        var rows = await _photoRepository.GetAllAsync(p => p.ID == id, page, pageSize);
        var photos = rows.Select(r =>
        {
            r.Path = $"{S3.Endpoint}/{S3.BucketName}/{r.Path}";
            return r;
        });

        return Ok(photos);
    }

    [HttpGet("profile/photos")]
    public async Task<IActionResult> GetAllAsync([FromHeader(Name = "user-id")] string userId, [FromQuery] Pagination pagination)
    {
        var (page, pageSize, _) = pagination;
        var rows = await _photoRepository.GetAllAsync(p => p.CustomerID == userId, page, pageSize);
        var photos = rows.Select(r =>
        {
            r.Path = $"{S3.Endpoint}/{S3.BucketName}/{r.Path}";
            return r;
        });

        return Ok(photos);
    }

    [HttpPost("profile/photos")]
    [RequestSizeLimit(2_000_000)]
    public async Task<IActionResult> CreateAsync([FromHeader(Name = "user-id")] string userId, [Required][FromForm] IFormFile image)
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
    public async Task<IActionResult> DeleteByIdAsync([FromHeader(Name = "user-id")] string userId, [FromRoute] string id)
    {
        var photo = await _photoRepository.GetAsync(p => p.CustomerID == userId && p.ID == id);

        if (photo is null) return NotFound();

        await _photoRepository.DeleteAsync(p => p.ID == photo.ID);
        Emitter<string>.EmitMessage(_operationHandler, photo.Path);

        return NoContent();
    }

    #endregion
}
