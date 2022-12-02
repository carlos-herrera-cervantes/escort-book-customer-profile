using System;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Xunit;
using EscortBookCustomerProfile.Web.Contexts;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Models;
using EscortBookCustomerProfile.Web.Constants;

namespace EscortBookCustomerProfile.Tests.Repositories;

[Category("Repositories")]
[Collection(nameof(ProfileRepository))]
[ExcludeFromCodeCoverage]
public class ProfileRepositoryTests
{
    #region snippet_Properties

    private readonly DbContextOptions<EscortBookCustomerProfileContext> _contextOptions;

    #endregion

    #region snippet_Constructors

    public ProfileRepositoryTests()
        => _contextOptions = new DbContextOptionsBuilder<EscortBookCustomerProfileContext>()
            .UseNpgsql(Environment.GetEnvironmentVariable("PG_DB_CONNECTION"))
            .Options;

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return an empty list")]
    public async Task GetAllAsyncShouldReturnEmptyList()
    {
        using var context = new EscortBookCustomerProfileContext(_contextOptions);
        var profileRepository = new ProfileRepository(context);

        IEnumerable<Profile> profiles = await profileRepository.GetAllAsync(1, 10);

        Assert.Empty(profiles);
    }

    [Fact(DisplayName = "Should apply all operations")]
    public async Task ShouldApplyCrudOperations()
    {
        using var context = new EscortBookCustomerProfileContext(_contextOptions);
        var profileRepository = new ProfileRepository(context);
        var profile = new Profile
        {
            CustomerID = "6389923b8c2862e0542ef177",
            Email = "test.customer.2@example.com"
        };

        await profileRepository.CreateAsync(profile);

        profile.Gender = Genders.Male;

        await profileRepository.UpdateAsync(profile);
        Profile getResult = await profileRepository.GetAsync(p => p.Email == "test.customer.2@example.com");

        Assert.True(getResult.Gender == Genders.Male);

        await profileRepository.DeleteAsync(p => p.Email == "test.customer.2@example.com");
        int counter = await profileRepository.CountAsync(p => p.Email == "test.customer.2@example.com");

        Assert.True(counter == 0);
    }

    #endregion
}
