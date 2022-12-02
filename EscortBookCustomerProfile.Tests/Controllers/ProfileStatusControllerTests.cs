using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Controllers;
using EscortBookCustomerProfile.Web.Handlers;
using EscortBookCustomerProfile.Web.Types;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Tests.Controllers;

[Category]
[Collection(nameof(ProfileStatusController))]
[ExcludeFromCodeCoverage]
public class ProfileStatusControllerTests
{
    #region snippet_Properties

    private readonly Mock<IProfileStatusRepository> _mockProfileStatusRepository;

    private readonly Mock<IProfileStatusCategoryRepository> _mockProfileStatusCategoryRepository;

    private readonly Mock<IOperationHandler<BlockUserEvent>> _mockDisableHandler;

    private readonly Mock<IOperationHandler<DeleteUserEvent>> _mockDeleteHandler;

    #endregion

    #region snippet_Constructors

    public ProfileStatusControllerTests()
    {
        _mockProfileStatusRepository = new Mock<IProfileStatusRepository>();
        _mockProfileStatusCategoryRepository = new Mock<IProfileStatusCategoryRepository>();
        _mockDisableHandler = new Mock<IOperationHandler<BlockUserEvent>>();
        _mockDeleteHandler = new Mock<IOperationHandler<DeleteUserEvent>>();
    }

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return 200 when listing categories")]
    public async Task GetProfileStatusCategoriesAsyncShouldReturn200()
    {
        _mockProfileStatusCategoryRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<ProfileStatusCategory>());

        var profileStatusController = new ProfileStatusController(
            _mockProfileStatusRepository.Object,
            _mockProfileStatusCategoryRepository.Object,
            _mockDisableHandler.Object,
            _mockDeleteHandler.Object
        );

        IActionResult res = await profileStatusController.GetProfileStatusCategoriesAsync();

        _mockProfileStatusCategoryRepository.Verify(x => x.GetAllAsync(), Times.Once);

        Assert.IsType<OkObjectResult>(res);
    }

    [Fact(DisplayName = "Should return 404 when profile status not found")]
    public async Task GetByExternalAsyncShouldReturn404ByProfileStatus()
    {
        _mockProfileStatusRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatus, bool>>>()))
            .ReturnsAsync(() => null);

        var profileStatusController = new ProfileStatusController(
            _mockProfileStatusRepository.Object,
            _mockProfileStatusCategoryRepository.Object,
            _mockDisableHandler.Object,
            _mockDeleteHandler.Object
        );

        IActionResult res = await profileStatusController.GetByExternalAsync(id: "63862df31a4d78eb39277e8b");

        _mockProfileStatusRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatus, bool>>>()), Times.Once);

        Assert.IsType<NotFoundObjectResult>(res);
    }

    [Fact(DisplayName = "Should return 404 when profile status category not found")]
    public async Task GetByExternalAsyncShouldReturn404ByCategory()
    {
        _mockProfileStatusRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatus, bool>>>()))
            .ReturnsAsync(new ProfileStatus());
        _mockProfileStatusCategoryRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatusCategory, bool>>>()))
            .ReturnsAsync(() => null);

        var profileStatusController = new ProfileStatusController(
            _mockProfileStatusRepository.Object,
            _mockProfileStatusCategoryRepository.Object,
            _mockDisableHandler.Object,
            _mockDeleteHandler.Object
        );

        IActionResult res = await profileStatusController.GetByExternalAsync(id: "63862df31a4d78eb39277e8b");

        _mockProfileStatusRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatus, bool>>>()), Times.Once);
        _mockProfileStatusCategoryRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatusCategory, bool>>>()), Times.Once);

        Assert.IsType<NotFoundObjectResult>(res);
    }

    [Fact(DisplayName = "Should return 200 when profile status exists")]
    public async Task GetByExternalAsyncShouldReturn200()
    {
        _mockProfileStatusRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatus, bool>>>()))
            .ReturnsAsync(new ProfileStatus());
        _mockProfileStatusCategoryRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatusCategory, bool>>>()))
            .ReturnsAsync(new ProfileStatusCategory());

        var profileStatusController = new ProfileStatusController(
            _mockProfileStatusRepository.Object,
            _mockProfileStatusCategoryRepository.Object,
            _mockDisableHandler.Object,
            _mockDeleteHandler.Object
        );

        IActionResult res = await profileStatusController.GetByExternalAsync(id: "63862df31a4d78eb39277e8b");

        _mockProfileStatusRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatus, bool>>>()), Times.Once);
        _mockProfileStatusCategoryRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatusCategory, bool>>>()), Times.Once);

        Assert.IsType<OkObjectResult>(res);
    }

    [Fact(DisplayName = "Should return 404 when profile status category does not exists")]
    public async Task UpdateByExternalAsyncShouldReturn404ByCategory()
    {
        _mockProfileStatusCategoryRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatusCategory, bool>>>()))
            .ReturnsAsync(() => null);

        var profileStatusController = new ProfileStatusController(
            _mockProfileStatusRepository.Object,
            _mockProfileStatusCategoryRepository.Object,
            _mockDisableHandler.Object,
            _mockDeleteHandler.Object
        );

        IActionResult res = await profileStatusController.UpdateByExternalAsync(
            id: "63862df31a4d78eb39277e8b",
            profile: new UpdateProfileStatus(),
            userEmail: "test.user@example.com",
            userType: "Customer"
        );

        _mockProfileStatusCategoryRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatusCategory, bool>>>()), Times.Once);

        Assert.IsType<NotFoundObjectResult>(res);
    }

    [Fact(DisplayName = "Should return 404 when profile status does not exists")]
    public async Task UpdateByExternalAsyncShouldReturn404ByProfileStatus()
    {
        _mockProfileStatusCategoryRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatusCategory, bool>>>()))
            .ReturnsAsync(new ProfileStatusCategory());
        _mockProfileStatusRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatus, bool>>>()))
            .ReturnsAsync(() => null);

        var profileStatusController = new ProfileStatusController(
            _mockProfileStatusRepository.Object,
            _mockProfileStatusCategoryRepository.Object,
            _mockDisableHandler.Object,
            _mockDeleteHandler.Object
        );

        IActionResult res = await profileStatusController.UpdateByExternalAsync(
            id: "63862df31a4d78eb39277e8b",
            profile: new UpdateProfileStatus(),
            userEmail: "test.user@example.com",
            userType: "Customer"
        );

        _mockProfileStatusCategoryRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatusCategory, bool>>>()), Times.Once);
        _mockProfileStatusRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatus, bool>>>()), Times.Once);

        Assert.IsType<NotFoundObjectResult>(res);
    }

    [Fact(DisplayName = "Should return 200 when profile status is updated successfully")]
    public async Task UpdateByExternalAsyncShouldReturn200()
    {
        _mockProfileStatusCategoryRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatusCategory, bool>>>()))
            .ReturnsAsync(new ProfileStatusCategory
            {
                Name = "Locked"
            });
        _mockProfileStatusRepository
            .Setup(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatus, bool>>>()))
            .ReturnsAsync(new ProfileStatus());
        _mockProfileStatusRepository
            .Setup(x => x.UpdateAsync(It.IsAny<ProfileStatus>()))
            .Returns(Task.CompletedTask);
        _mockDisableHandler.Setup(x => x.Publish(It.IsAny<BlockUserEvent>()));

        var profileStatusController = new ProfileStatusController(
            _mockProfileStatusRepository.Object,
            _mockProfileStatusCategoryRepository.Object,
            _mockDisableHandler.Object,
            _mockDeleteHandler.Object
        );

        IActionResult res = await profileStatusController.UpdateByExternalAsync(
            id: "63862df31a4d78eb39277e8b",
            profile: new UpdateProfileStatus(),
            userEmail: "test.user@example.com",
            userType: "Customer"
        );

        _mockProfileStatusCategoryRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatusCategory, bool>>>()), Times.Once);
        _mockProfileStatusRepository
            .Verify(x => x.GetAsync(It.IsAny<Expression<Func<ProfileStatus, bool>>>()), Times.Once);
        _mockProfileStatusRepository.Verify(x => x.UpdateAsync(It.IsAny<ProfileStatus>()), Times.Once);
        _mockDisableHandler.Verify(x => x.Publish(It.IsAny<BlockUserEvent>()), Times.Once);

        Assert.IsType<OkObjectResult>(res);
    }

    #endregion
}
