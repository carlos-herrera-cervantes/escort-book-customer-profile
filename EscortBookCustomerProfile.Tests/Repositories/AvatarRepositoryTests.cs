using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Contexts;
using EscortBookCustomerProfile.Web.Models;

namespace EscortBookCustomerProfile.Tests.Repositories;

[Category("Repositories")]
[Collection(nameof(AvatarRepository))]
[ExcludeFromCodeCoverage]
public class AvatarRepositoryTests
{
    #region snippet_Properties

    private readonly DbContextOptions<EscortBookCustomerProfileContext> _contextOptions;

    #endregion

    #region snippet_Constructors

    public AvatarRepositoryTests()
        => _contextOptions = new DbContextOptionsBuilder<EscortBookCustomerProfileContext>()
            .UseNpgsql(Environment.GetEnvironmentVariable("PG_DB_CONNECTION"))
            .Options;

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should apply all operations")]
    public async Task ShouldApplyCrudOperations()
    {
        using var context = new EscortBookCustomerProfileContext(_contextOptions);
        var avatarRepository = new AvatarRepository(context);
        var profileRepository = new ProfileRepository(context);
        var profile = new Profile
        {
            CustomerID = "63883b85e8cbfc4d94ade63f",
            Email = "test.customer@example.com"
        };
        var avatar = new Avatar
        {
            ID = "63883b4ee9523982d2c842c3",
            Path = "profile.png",
            CustomerID = "63883b85e8cbfc4d94ade63f"
        };

        await profileRepository.CreateAsync(profile);
        await avatarRepository.CreateAsync(avatar);
        Avatar getResultBeforeUpdate = await avatarRepository.GetAsync(a => a.ID == "63883b4ee9523982d2c842c3");

        Assert.Equal("profile.png", getResultBeforeUpdate.Path);

        var jsonPatch = new JsonPatchDocument<Avatar>();
        jsonPatch.Replace(a => a.Path, "new-profile.png");

        await avatarRepository.UpdateAsync(avatar, jsonPatch);
        Avatar getResultAfterUpdate = await avatarRepository.GetAsync(a => a.ID == "63883b4ee9523982d2c842c3");

        Assert.Equal("new-profile.png", getResultAfterUpdate.Path);

        await avatarRepository.DeleteAsync(a => a.ID == "63883b4ee9523982d2c842c3");
        int counter = await avatarRepository.CountAsync(a => a.ID == "63883b4ee9523982d2c842c3");

        Assert.Equal<int>(0, counter);

        await profileRepository.DeleteAsync(a => a.CustomerID == "63883b85e8cbfc4d94ade63f");
    }

    #endregion
}
