using System.ComponentModel;
using Xunit;
using System.Diagnostics.CodeAnalysis;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Amazon.S3;
using Amazon.S3.Model;
using System.Threading;
using EscortBookCustomerProfile.Web.Services;
using EscortBookCustomerProfile.Web.Constants;

namespace EscortBookCustomerProfile.Tests.Services;

[Category("Services")]
[Collection(nameof(AWSS3Service))]
[ExcludeFromCodeCoverage]
public class AWSS3ServiceTests
{
    #region snippet_Properties

    private readonly Mock<IAmazonS3> _mockS3Service;

    private readonly Mock<IFormFile> _mockFormFile;

    #endregion

    #region snippet_Constructors

    public AWSS3ServiceTests()
        => (_mockS3Service, _mockFormFile) = (new Mock<IAmazonS3>(), new Mock<IFormFile>());

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return the URL with the full path to the file")]
    public async Task PutObjectAsyncShouldReturnString()
    {
        _mockS3Service
            .Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()));

        var s3Service = new AWSS3Service(_mockS3Service.Object);

        string url = await s3Service.PutObjectAsync(
            key: "profile.png",
            profileId: "63850d910b43fadcaedcd106",
            imageStream: _mockFormFile.Object.OpenReadStream()
        );

        _mockS3Service
            .Verify(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        Assert.True(url == $"{S3.Endpoint}/{S3.BucketName}/63850d910b43fadcaedcd106/profile.png");
    }

    [Fact(DisplayName = "Should successfully invoke DeleteObjectAsync method of the service")]
    public async Task DeleteObjectAsyncShouldBeSuccessfully()
    {
        _mockS3Service
            .Setup(x => x.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), It.IsAny<CancellationToken>()));

        var s3Service = new AWSS3Service(_mockS3Service.Object);

        await s3Service.DeleteObjectAsync(key: "profile.png");

        _mockS3Service
            .Verify(
                x => x.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
    }

    #endregion
}
