using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Globalization;
using System;
using Microsoft.EntityFrameworkCore;
using Xunit;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Contexts;
using EscortBookCustomerProfile.Web.Models;
using EscortBookCustomerProfile.Web.Constants;

namespace EscortBookCustomerProfile.Tests.Repositories;

[Category("Repositories")]
[Collection(nameof(ProfileStatusRepository))]
[ExcludeFromCodeCoverage]
public class ProfileStatusRepositoryTests
{
    #region snippet_Properties

    private readonly DbContextOptions<EscortBookCustomerProfileContext> _contextOptions;

    #endregion

    #region snippet_Constructors

    public ProfileStatusRepositoryTests()
        => _contextOptions = new DbContextOptionsBuilder<EscortBookCustomerProfileContext>()
            .UseNpgsql(PostgresDatabase.CustomerProfile)
            .Options;

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should apply all operations")]
    public async Task ShouldApplyCrudOperations()
    {
        var profile = new Profile
        {
            CustomerID = "638151b282dea38826eab954",
            Email = "test.user2@example.com",
            Birthdate = DateTime.ParseExact("1990-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture)
        };
        var category1 = new ProfileStatusCategory
        {
            Name = "Test Category 1"
        };
        var category2 = new ProfileStatusCategory
        {
            Name = "Test Category 2"
        };

        using var context = new EscortBookCustomerProfileContext(_contextOptions);
        var profileRepository = new ProfileRepository(context);
        var profileStatusCategoryRepository = new ProfileStatusCategoryRepository(context);

        await profileRepository.CreateAsync(profile);
        await profileStatusCategoryRepository.CreateAsync(category1);
        await profileStatusCategoryRepository.CreateAsync(category2);

        var profileStatus = new ProfileStatus
        {
            ID = "a8da088a-0fad-4a57-9fb0-118f1e638e5a",
            CustomerID = "638151b282dea38826eab954",
            ProfileStatusCategoryID = category1.ID
        };

        var profileStatusRepository = new ProfileStatusRepository(context);
        await profileStatusRepository.CreateAsync(profileStatus);

        profileStatus.ProfileStatusCategoryID = category2.ID;
        await profileStatusRepository.UpdateAsync(profileStatus);

        ProfileStatus getResult = await profileStatusRepository.GetAsync(ps => ps.CustomerID == "638151b282dea38826eab954");
        Assert.Equal(category2.ID, getResult.ProfileStatusCategoryID);

        await profileStatusRepository.DeleteAsync(ps => ps.ID == "a8da088a-0fad-4a57-9fb0-118f1e638e5a");
        int counterAfterDelete = await profileStatusRepository.CountAsync(ps => ps.CustomerID == "638151b282dea38826eab954");
        Assert.True(counterAfterDelete == 0);

        await profileRepository.DeleteAsync(p => p.CustomerID == "638151b282dea38826eab954");
        await profileStatusCategoryRepository.DeleteAsync(c => c.Name != string.Empty);
    }

    #endregion
}
