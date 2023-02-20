using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Xunit;
using Moq;
using EscortBookCustomerProfile.Web.Controllers;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Services;
using EscortBookCustomerProfile.Web.Handlers;
using EscortBookCustomerProfile.Web.Models;
using EscortBookCustomerProfile.Web.Constants;

namespace EscortBookCustomerProfile.Tests.Controllers;

[Category]
[Collection(nameof(PhotoController))]
[ExcludeFromCodeCoverage]
public class PhotoControllerTests
{
    #region snippet_Properties

    private readonly Mock<IPhotoRepository> _mockPhotoRepository;

    private readonly Mock<IAWSS3Service> _mockS3Service;

    private readonly Mock<IOperationHandler<string>> _mockOperationHandler;

    private readonly Mock<IFormFile> _mockFormFile;

    #endregion

    #region snippet_Constructors

    public PhotoControllerTests()
    {
        _mockPhotoRepository = new Mock<IPhotoRepository>();
        _mockS3Service = new Mock<IAWSS3Service>();
        _mockOperationHandler = new Mock<IOperationHandler<string>>();
        _mockFormFile = new Mock<IFormFile>();
    }

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return 200 when listing photos")]
    public async Task GetAllAsyncShouldReturn200()
    {
        _mockPhotoRepository
            .Setup(x => x.GetAllAsync(
                It.IsAny<Expression<Func<Photo, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>()
            ))
            .ReturnsAsync(new List<Photo>
            {
                new Photo
                {
                    Path = "profile.png"
                },
            });

        var photoController = new PhotoController(
            _mockPhotoRepository.Object,
            _mockS3Service.Object,
            _mockOperationHandler.Object
        );

        IActionResult res = await photoController.GetAllAsync(userId: "63845870dc7d494b6c7a9a05", new Pagination());
        var okObjectResult = res as OkObjectResult;
        var body = okObjectResult?.Value as IEnumerable<Photo>;

        _mockPhotoRepository
            .Verify(x => x.GetAllAsync(
                It.IsAny<Expression<Func<Photo, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>()
            ), Times.Once);

        Assert.IsType<OkObjectResult>(res);
        Assert.True(body?.FirstOrDefault()?.Path == $"{S3.Endpoint}/{S3.BucketName}/profile.png");
    }

    [Fact(DisplayName = "Should return 201 when photo loads successfully")]
    public async Task CreateAsyncShouldReturn201()
    {
        _mockS3Service
            .Setup(x => x.PutObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync((string key, string userId, Stream image) => $"{S3.Endpoint}/{S3.BucketName}/{key}");
        _mockPhotoRepository.Setup(x => x.CreateAsync(It.IsAny<Photo>())).Returns(Task.CompletedTask);
        _mockFormFile.Setup(x => x.FileName).Returns("profile.png");

        var photoController = new PhotoController(
            _mockPhotoRepository.Object,
            _mockS3Service.Object,
            _mockOperationHandler.Object
        );

        IActionResult res = await photoController.CreateAsync(userId: "63845870dc7d494b6c7a9a05", _mockFormFile.Object);
        var createdResult = res as CreatedResult;
        var body = createdResult?.Value as Photo;

        _mockS3Service
            .Verify(x => x.PutObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);
        _mockPhotoRepository.Verify(x => x.CreateAsync(It.IsAny<Photo>()), Times.Once);

        Assert.IsType<CreatedResult>(res);
        Assert.True(body?.Path == $"{S3.Endpoint}/{S3.BucketName}/profile.png");
    }

    [Fact(DisplayName = "Should return 404 when photo does not exists")]
    public async Task DeleteByIdAsync()
    {
        _mockPhotoRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Photo, bool>>>()))
            .ReturnsAsync(() => null);

        var photoController = new PhotoController(
            _mockPhotoRepository.Object,
            _mockS3Service.Object,
            _mockOperationHandler.Object
        );

        IActionResult res = await photoController.DeleteByIdAsync(
            userId: "63845870dc7d494b6c7a9a05",
            id: "63845d7b6c92a40c1767d9ad"
        );

        _mockPhotoRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<Photo, bool>>>()), Times.Once);

        Assert.IsType<NotFoundResult>(res);
    }

    [Fact(DisplayName = "Should return 204 when photo is deleted successfully")]
    public async Task DeleteByIdAsyncShouldReturn204()
    {
        _mockPhotoRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Photo, bool>>>()))
            .ReturnsAsync(new Photo());
        _mockPhotoRepository
            .Setup(x => x.DeleteAsync(It.IsAny<Expression<Func<Photo, bool>>>()))
            .Returns(Task.CompletedTask);
        _mockOperationHandler.Setup(x => x.Publish(It.IsAny<string>()));

        var photoController = new PhotoController(
            _mockPhotoRepository.Object,
            _mockS3Service.Object,
            _mockOperationHandler.Object
        );

        IActionResult res = await photoController.DeleteByIdAsync(
            userId: "63845870dc7d494b6c7a9a05",
            id: "63845d7b6c92a40c1767d9ad"
        );

        _mockPhotoRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<Photo, bool>>>()), Times.Once);
        _mockPhotoRepository.Verify(x => x.DeleteAsync(It.IsAny<Expression<Func<Photo, bool>>>()), Times.Once);
        _mockOperationHandler.Verify(x => x.Publish(It.IsAny<string>()), Times.Once);

        Assert.IsType<NoContentResult>(res);
    }

    #endregion
}
