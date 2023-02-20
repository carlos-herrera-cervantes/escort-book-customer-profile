using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Xunit;
using Moq;
using EscortBookCustomerProfile.Web.Controllers;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Services;
using EscortBookCustomerProfile.Web.Models;
using EscortBookCustomerProfile.Web.Constants;

namespace EscortBookCustomerProfile.Tests.Controllers;

[Category("Controllers")]
[Collection(nameof(AvatarController))]
[ExcludeFromCodeCoverage]
public class AvatarControllerTests
{
    #region snippet_Properties

    private readonly Mock<IAvatarRepository> _mockAvatarRepository;

    private readonly Mock<IAWSS3Service> _mockS3Service;

    private readonly Mock<IFormFile> _mockFormFile;

    #endregion

    #region snippet_Constructors

    public AvatarControllerTests()
    {
        _mockAvatarRepository = new Mock<IAvatarRepository>();
        _mockS3Service = new Mock<IAWSS3Service>();
        _mockFormFile = new Mock<IFormFile>();
    }

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return 404 when avatar does not exists")]
    public async Task GetByExternalAsyncShouldReturn404()
    {
        _mockAvatarRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Avatar, bool>>>()))
            .ReturnsAsync(() => null);

        var avatarController = new AvatarController(_mockAvatarRepository.Object, _mockS3Service.Object);

        IActionResult res = await avatarController.GetByExternalAsync(id: "6382727e12a820ffa6932870");

        _mockAvatarRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<Avatar, bool>>>()), Times.Once);

        Assert.IsType<NotFoundResult>(res);
    }

    [Fact(DisplayName = "Should return 200 when avatar exists")]
    public async Task GetByExternalAsyncShouldReturn200()
    {
        _mockAvatarRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Avatar, bool>>>()))
            .ReturnsAsync(() => new Avatar
            {
                Path = "profile.png"
            });

        var avatarController = new AvatarController(_mockAvatarRepository.Object, _mockS3Service.Object);

        IActionResult res = await avatarController.GetByExternalAsync(id: "6382727e12a820ffa6932870");
        var okObjectResult = res as OkObjectResult;
        var body = okObjectResult?.Value as Avatar;

        _mockAvatarRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<Avatar, bool>>>()), Times.Once);

        Assert.IsType<OkObjectResult>(res);
        Assert.True(body?.Path == $"{S3.Endpoint}/{S3.BucketName}/profile.png");
    }

    [Fact(DisplayName = "Should return 201 when avatar is created successfully")]
    public async Task CreateAsyncShouldReturn201()
    {
        _mockS3Service
            .Setup(x => x.PutObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync("http://localhost:4566/test-bucket/profile.png");
        _mockAvatarRepository.Setup(x => x.CreateAsync(It.IsAny<Avatar>())).Returns(Task.CompletedTask);
        _mockFormFile.Setup(x => x.FileName).Returns("profile.png");

        var avatarController = new AvatarController(_mockAvatarRepository.Object, _mockS3Service.Object);

        IActionResult res = await avatarController.CreateAsync(
            image: _mockFormFile.Object,
            userId: "6382a27427b5f682cc6354c2"
        );
        var createdResult = res as CreatedResult;
        var avatar = createdResult?.Value as Avatar;

        _mockS3Service
            .Verify(x => x.PutObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);
        _mockAvatarRepository.Verify(x => x.CreateAsync(It.IsAny<Avatar>()), Times.Once);

        Assert.IsType<CreatedResult>(res);
        Assert.True(avatar?.Path == "http://localhost:4566/test-bucket/profile.png");
    }

    [Fact(DisplayName = "Should return 404 when avatar does not exists")]
    public async Task UpdateByIdAsyncShouldReturn404()
    {
        _mockAvatarRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Avatar, bool>>>()))
            .ReturnsAsync(() => null);

        var avatarController = new AvatarController(_mockAvatarRepository.Object, _mockS3Service.Object);

        IActionResult res = await avatarController.UpdateByIdAsync(
            image: _mockFormFile.Object,
            userId: "6382a27427b5f682cc6354c2"
        );

        _mockAvatarRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<Avatar, bool>>>()), Times.Once);

        Assert.IsType<NotFoundResult>(res);
    }

    [Fact(DisplayName = "Should return 200 when avatar is updated successfully")]
    public async Task UpdateByIdAsyncShouldReturn200()
    {
        _mockAvatarRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Avatar, bool>>>()))
            .ReturnsAsync(new Avatar());
        _mockAvatarRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Avatar>(), It.IsAny<JsonPatchDocument<Avatar>>()))
            .Returns(Task.CompletedTask);
        _mockS3Service
            .Setup(x => x.PutObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync("http://localhost:4566/test-bucket/profile.png");
        _mockFormFile.Setup(x => x.FileName).Returns("profile.png");

        var avatarController = new AvatarController(_mockAvatarRepository.Object, _mockS3Service.Object);

        IActionResult res = await avatarController.UpdateByIdAsync(
            image: _mockFormFile.Object,
            userId: "6382a27427b5f682cc6354c2"
        );
        var okObjectResult = res as OkObjectResult;
        var body = okObjectResult?.Value as Avatar;

        _mockAvatarRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<Avatar, bool>>>()), Times.Once);
        _mockAvatarRepository
            .Verify(x => x.UpdateAsync(It.IsAny<Avatar>(), It.IsAny<JsonPatchDocument<Avatar>>()), Times.Once);
        _mockS3Service
            .Verify(x => x.PutObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);

        Assert.IsType<OkObjectResult>(res);
        Assert.True(body?.Path == "http://localhost:4566/test-bucket/profile.png");
    }

    [Fact(DisplayName = "Should return 404 when avatar does not exists")]
    public async Task DeleteByIdAsyncShouldReturn404()
    {
        _mockAvatarRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Avatar, bool>>>()))
            .ReturnsAsync(() => null);

        var avatarController = new AvatarController(_mockAvatarRepository.Object, _mockS3Service.Object);

        IActionResult res = await avatarController.DeleteByIdAsync(userId: "6382a27427b5f682cc6354c2");

        _mockAvatarRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<Avatar, bool>>>()), Times.Once);

        Assert.IsType<NotFoundResult>(res);
    }

    [Fact(DisplayName = "Should return 204 when avatar is deleted successfully")]
    public async Task DeleteByIdAsyncShouldReturn204()
    {
        _mockAvatarRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Avatar, bool>>>()))
            .ReturnsAsync(new Avatar());
        _mockAvatarRepository
            .Setup(x => x.DeleteAsync(It.IsAny<Expression<Func<Avatar, bool>>>()))
            .Returns(Task.CompletedTask);

        var avatarController = new AvatarController(_mockAvatarRepository.Object, _mockS3Service.Object);

        IActionResult res = await avatarController.DeleteByIdAsync(userId: "6382a27427b5f682cc6354c2");

        _mockAvatarRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<Avatar, bool>>>()), Times.Once);
        _mockAvatarRepository
            .Verify(x => x.DeleteAsync(It.IsAny<Expression<Func<Avatar, bool>>>()), Times.Once);

        Assert.IsType<NoContentResult>(res);
    }

    #endregion
}
