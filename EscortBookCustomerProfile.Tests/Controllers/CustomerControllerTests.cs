using System.ComponentModel;
using Xunit;
using System.Diagnostics.CodeAnalysis;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Controllers;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Tests.Controllers;

[Category("Controllers")]
[Collection(nameof(CustomerController))]
[ExcludeFromCodeCoverage]
public class CustomerControllerTests
{
    #region snippet_Properties

    private readonly Mock<IProfileRepository> _mockProfileRepository;

    #endregion

    #region snippet_Constructors

    public CustomerControllerTests()
    {
        _mockProfileRepository = new Mock<IProfileRepository>();
    }

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return 200 when listing profiles")]
    public async Task GetAllAsyncShouldReturn200()
    {
        _mockProfileRepository
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<Profile>());
        _mockProfileRepository
            .Setup(x => x.CountAsync(It.IsAny<Expression<Func<Profile, bool>>>()))
            .ReturnsAsync(0);

        var customerController = new CustomerController(_mockProfileRepository.Object);

        IActionResult res = await customerController.GetAllAsync(new Pagination());
        var okObjectResult = res as OkObjectResult;
        var body = okObjectResult?.Value as PaginationResult<Profile>;

        _mockProfileRepository
            .Verify(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        _mockProfileRepository
            .Verify(x => x.CountAsync(It.IsAny<Expression<Func<Profile, bool>>>()), Times.Once);

        Assert.IsType<OkObjectResult>(res);
        Assert.True(body?.Total == 0);
        Assert.True(body?.Next == 0);
        Assert.True(body?.Previous == 0);
        Assert.Empty(body?.Data);
    }

    #endregion
}
