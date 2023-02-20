using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using Xunit;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Contexts;
using EscortBookCustomerProfile.Web.Models;
using EscortBookCustomerProfile.Web.Constants;

namespace EscortBookCustomerProfile.Tests.Repositories;

[Category("Repositories")]
[Collection(nameof(ProfileStatusCategoryRepository))]
[ExcludeFromCodeCoverage]
public class ProfileStatusCategoryRepositoryTests
{
    #region snippet_Properties

    private readonly DbContextOptions<EscortBookCustomerProfileContext> _contextOptions;

    #endregion

    #region snippet_Constructors

    public ProfileStatusCategoryRepositoryTests()
        => _contextOptions = new DbContextOptionsBuilder<EscortBookCustomerProfileContext>()
            .UseNpgsql(PostgresDatabase.CustomerProfile)
            .Options;

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return an empty list")]
    public async Task GetAllAsyncShouldReturnEmptyList()
    {
        using var context = new EscortBookCustomerProfileContext(_contextOptions);
        var profileStatusCategoryRepository = new ProfileStatusCategoryRepository(context);

        IEnumerable<ProfileStatusCategory> categories = await profileStatusCategoryRepository.GetAllAsync();

        Assert.Empty(categories);
    }

    [Fact(DisplayName = "Should return null when record does not exists")]
    public async Task GetAsyncShouldReturnNull()
    {
        using var context = new EscortBookCustomerProfileContext(_contextOptions);
        var profileStatusCategoryRepository = new ProfileStatusCategoryRepository(context);

        ProfileStatusCategory category = await profileStatusCategoryRepository
            .GetAsync(c => c.ID == "6389824fd59ebd23b4eb32b7");

        Assert.Null(category);
    }

    #endregion
}
