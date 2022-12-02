using System.ComponentModel;
using Xunit;
using System.Diagnostics.CodeAnalysis;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Controllers;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Tests.Controllers;

[Category]
[Collection(nameof(ProfileController))]
[ExcludeFromCodeCoverage]
public class ProfileControllerTests
{
    #region snippet_Properties

    private readonly Mock<IProfileRepository> _mockProfileRepository;

    #endregion

    #region snippet_Constructors

    public ProfileControllerTests()
    {
        _mockProfileRepository = new Mock<IProfileRepository>();
    }

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return 404 when profile does not exists")]
    public async Task GetByIdAsyncShouldReturn404()
    {
        _mockProfileRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Profile, bool>>>()))
            .ReturnsAsync(() => null);

        var profileController = new ProfileController(_mockProfileRepository.Object);

        IActionResult res = await profileController.GetByIdAsync(userId: "6384663356abb298f06d6745");

        _mockProfileRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<Profile, bool>>>()), Times.Once);

        Assert.IsType<NotFoundResult>(res);
    }

    [Fact(DisplayName = "Should return 200 when profile exists")]
    public async Task GetByIdAsyncShouldReturn200()
    {
        _mockProfileRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Profile, bool>>>()))
            .ReturnsAsync(new Profile());

        var profileController = new ProfileController(_mockProfileRepository.Object);

        IActionResult res = await profileController.GetByIdAsync(userId: "6384663356abb298f06d6745");

        _mockProfileRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<Profile, bool>>>()), Times.Once);

        Assert.IsType<OkObjectResult>(res);
    }

    [Fact(DisplayName = "Should return 404 when profile does not exists")]
    public async Task UpdateByIdAsyncShouldReturn404()
    {
        _mockProfileRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<Profile, bool>>>()))
            .ReturnsAsync(() => null);

        var profileController = new ProfileController(_mockProfileRepository.Object);

        IActionResult res = await profileController.UpdateByIdAsync(
            new UpdateProfile(),
            userId: "6384663356abb298f06d6745"
        );

        _mockProfileRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<Profile, bool>>>()), Times.Once);

        Assert.IsType<NotFoundResult>(res);
    }

    #endregion
}
