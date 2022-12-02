using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using EscortBookCustomerProfile.Web.Models;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Services;
using EscortBookCustomerProfile.Web.Constants;

namespace EscortBookCustomerProfile.Web.Controllers;

[Route("api/v1/customer")]
[ApiController]
public class AvatarController : ControllerBase
{
    #region snippet_Properties

    private readonly IAvatarRepository _avatarRepository;

    private readonly IAWSS3Service _s3Service;

    #endregion

    #region snippet_Constructors

    public AvatarController(IAvatarRepository avatarRepository, IAWSS3Service s3Service)
    {
        _avatarRepository = avatarRepository;
        _s3Service = s3Service;
    }

    #endregion

    #region snippet_ActionMethods

    [HttpGet("{id}/profile/avatar")]
    public async Task<IActionResult> GetByExternalAsync([FromRoute] string id)
    {
        var avatar = await _avatarRepository.GetAsync(a => a.ID == id);

        if (avatar is null) return NotFound();

        avatar.Path = $"{S3.Endpoint}/{S3.BucketName}/{avatar.Path}";

        return Ok(avatar);
    }

    [HttpGet("profile/avatar")]
    [ExcludeFromCodeCoverage]
    public async Task<IActionResult> GetByIdAsync([FromHeader(Name = "user-id")] string userId)
    {
        var avatar = await _avatarRepository.GetAsync(a => a.CustomerID == userId);

        if (avatar is null) return NotFound();

        avatar.Path = $"{S3.Endpoint}/{S3.BucketName}/{avatar.Path}";

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
        var newAvatar = await _avatarRepository.GetAsync(a => a.CustomerID == userId);

        if (newAvatar is null) return NotFound();

        var imageStream = image.OpenReadStream();
        var url = await _s3Service.PutObjectAsync(image.FileName, userId, imageStream);

        var currentAvatar = new JsonPatchDocument<Avatar>();
        currentAvatar.Replace(a => a.Path, $"{userId}/{image.FileName}");

        await _avatarRepository.UpdateAsync(newAvatar, currentAvatar);

        newAvatar.Path = url;

        return Ok(newAvatar);
    }

    [HttpDelete("profile/avatar")]
    public async Task<IActionResult> DeleteByIdAsync([FromHeader(Name = "user-id")] string userId)
    {
        var avatar = await _avatarRepository.GetAsync(a => a.CustomerID == userId);

        if (avatar is null) return NotFound();

        await _avatarRepository.DeleteAsync(a => a.ID == avatar.ID);

        return NoContent();
    }

    #endregion
}
