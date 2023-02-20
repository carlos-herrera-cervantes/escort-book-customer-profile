using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Models;
using EscortBookCustomerProfile.Web.Controllers;

namespace EscortBookCustomerProfile.Tests.Controllers;

[Category("Controllers")]
[Collection(nameof(IdentificationPartController))]
[ExcludeFromCodeCoverage]
public class IdentificationPartControllerTests
{
    #region snippet_Properties

    private readonly Mock<IIdentificationPartRepository> _mockIdentificationPartRepository;

    #endregion

    #region snippet_Constructors

    public IdentificationPartControllerTests()
    {
        _mockIdentificationPartRepository = new Mock<IIdentificationPartRepository>();
    }

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return 200 when listing parts")]
    public async Task GetAllAsyncShouldReturn200()
    {
        _mockIdentificationPartRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<IdentificationPart>());

        var identificationPartController = new IdentificationPartController(_mockIdentificationPartRepository.Object);

        IActionResult res = await identificationPartController.GetAllAsync();

        _mockIdentificationPartRepository.Verify(x => x.GetAllAsync(), Times.Once);

        Assert.IsType<OkObjectResult>(res);
    }

    #endregion
}
