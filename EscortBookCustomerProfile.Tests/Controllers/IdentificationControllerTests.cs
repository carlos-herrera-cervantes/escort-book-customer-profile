using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
[Collection(nameof(IdentificationController))]
[ExcludeFromCodeCoverage]
public class IdentificationControllerTests
{
    #region snippet_Properties

    private readonly Mock<IIdentificationRepository> _mockIdentificationRpository;

    private readonly Mock<IAWSS3Service> _mockS3Service;

    private readonly Mock<IFormFile> _mockFormFile;

    #endregion

    #region snippet_Constructors

    public IdentificationControllerTests()
    {
        _mockIdentificationRpository = new Mock<IIdentificationRepository>();
        _mockS3Service = new Mock<IAWSS3Service>();
        _mockFormFile = new Mock<IFormFile>();
    }

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return 200 when all identifications are successfully listed")]
    public async Task GetByExternalAsyncShouldReturn200()
    {
        var identification = new Identification
        {
            Path = "profile.png"
        };
        _mockIdentificationRpository
            .Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Identification, bool>>>()))
            .ReturnsAsync(new List<Identification> { identification });

        var identificationController = new IdentificationController(
            _mockIdentificationRpository.Object,
            _mockS3Service.Object
        );

        IActionResult res = await identificationController.GetByExternalAsync(id: "6383b106c303dba68af7829f");
        var okObjectResult = res as OkObjectResult;
        var identifications = okObjectResult?.Value as IEnumerable<Identification>;

        _mockIdentificationRpository
            .Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<Identification, bool>>>()), Times.Once);

        Assert.IsType<OkObjectResult>(res);
        Assert.True(identifications?.FirstOrDefault()?.Path == $"{S3.Endpoint}/{S3.BucketName}/profile.png");
    }

    [Fact(DisplayName = "Should return 404 when identification does not exists")]
    public async Task GetByIdAsyncShouldReturn404()
    {
        _mockIdentificationRpository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Identification, bool>>>()))
            .ReturnsAsync(() => null);

        var identificationController = new IdentificationController(
            _mockIdentificationRpository.Object,
            _mockS3Service.Object
        );

        IActionResult res = await identificationController.GetByIdAsync(
            userId: "6383b5ec7627d31dfbdf2cf6",
            identificationPartID: "6383b5f3cf15bf458bdf5b97"
        );

        _mockIdentificationRpository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<Identification, bool>>>()), Times.Once);

        Assert.IsType<NotFoundResult>(res);
    }

    [Fact(DisplayName = "Should return 200 when identification exists")]
    public async Task GetByIdAsyncShouldReturn200()
    {
        _mockIdentificationRpository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Identification, bool>>>()))
            .ReturnsAsync(new Identification
            {
                Path = "profile.png"
            });

        var identificationController = new IdentificationController(
            _mockIdentificationRpository.Object,
            _mockS3Service.Object
        );

        IActionResult res = await identificationController.GetByIdAsync(
            userId: "6383b5ec7627d31dfbdf2cf6",
            identificationPartID: "6383b5f3cf15bf458bdf5b97"
        );
        var okObjectResult = res as OkObjectResult;
        var body = okObjectResult?.Value as Identification;

        _mockIdentificationRpository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<Identification, bool>>>()), Times.Once);

        Assert.IsType<OkObjectResult>(res);
        Assert.True(body?.Path == $"{S3.Endpoint}/{S3.BucketName}/profile.png");
    }

    [Fact(DisplayName = "Should return 201 when identification is created successfully")]
    public async Task CreateAsyncShouldReturn201()
    {
        _mockS3Service
            .Setup(x => x.PutObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync((string key, string profileId, Stream image) => $"http://localhost:4566/test-bucket/{key}");
        _mockIdentificationRpository
            .Setup(x => x.CreateAsync(It.IsAny<Identification>()))
            .Returns(Task.CompletedTask);
        _mockFormFile.Setup(x => x.FileName).Returns("profile.png");

        var identificationController = new IdentificationController(
            _mockIdentificationRpository.Object,
            _mockS3Service.Object
        );

        IActionResult res = await identificationController
            .CreateAsync(userId: "6383b5ec7627d31dfbdf2cf6", new CreateIdentification
            {
                IdentificationPartID = "6383b5f3cf15bf458bdf5b97",
                Image = _mockFormFile.Object
            });
        var createdResult = res as CreatedResult;
        var body = createdResult?.Value as Identification;

        Assert.IsType<CreatedResult>(res);
        Assert.True(body?.CustomerID == "6383b5ec7627d31dfbdf2cf6");
        Assert.True(body?.Path == $"http://localhost:4566/test-bucket/profile.png");
        Assert.True(body?.IdentificationPartID == "6383b5f3cf15bf458bdf5b97");
    }

    [Fact(DisplayName = "Should return 404 identification does not exists")]
    public async Task UpdateByIdAsyncShouldReturn404()
    {
        _mockIdentificationRpository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Identification, bool>>>()))
            .ReturnsAsync(() => null);

        var identificationController = new IdentificationController(
            _mockIdentificationRpository.Object,
            _mockS3Service.Object
        );

        IActionResult res = await identificationController.UpdateByIdAsync(
            userId: "6383b5ec7627d31dfbdf2cf6",
            identificationPartID: "6383b5f3cf15bf458bdf5b97",
            image: _mockFormFile.Object
        );

        Assert.IsType<NotFoundResult>(res);
    }

    [Fact(DisplayName = "Should return 200 when identification is updated successfully")]
    public async Task UpdateByIdAsyncShouldReturn200()
    {
        _mockIdentificationRpository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Identification, bool>>>()))
            .ReturnsAsync(new Identification());
        _mockS3Service
            .Setup(x => x.PutObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync((string key, string profileId, Stream image) => $"http://localhost:4566/test-bucket/{key}");
        _mockIdentificationRpository
            .Setup(x => x.UpdateAsync(It.IsAny<Identification>(), It.IsAny<JsonPatchDocument<Identification>>()))
            .Returns(Task.CompletedTask);
        _mockFormFile.Setup(x => x.FileName).Returns("profile.png");

        var identificationController = new IdentificationController(
            _mockIdentificationRpository.Object,
            _mockS3Service.Object
        );

        IActionResult res = await identificationController.UpdateByIdAsync(
            userId: "6383b5ec7627d31dfbdf2cf6",
            identificationPartID: "6383b5f3cf15bf458bdf5b97",
            image: _mockFormFile.Object
        );
        var okObjectResult = res as OkObjectResult;
        var body = okObjectResult?.Value as Identification;

        _mockIdentificationRpository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<Identification, bool>>>()), Times.Once);
        _mockS3Service
            .Verify(x => x.PutObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);
        _mockIdentificationRpository
            .Verify(x => x.UpdateAsync(It.IsAny<Identification>(), It.IsAny<JsonPatchDocument<Identification>>()), Times.Once);

        Assert.IsType<OkObjectResult>(res);
        Assert.True(body?.Path == $"http://localhost:4566/test-bucket/profile.png");
    }

    #endregion
}
