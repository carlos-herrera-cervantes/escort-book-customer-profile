using System;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Xunit;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Contexts;
using EscortBookCustomerProfile.Web.Models;
using EscortBookCustomerProfile.Web.Constants;

namespace EscortBookCustomerProfile.Tests.Repositories;

[Category("Repositories")]
[Collection(nameof(PhotoRepository))]
[ExcludeFromCodeCoverage]
public class PhotoRepositoryTests
{
    #region snippet_Properties

    private readonly DbContextOptions<EscortBookCustomerProfileContext> _contextOptions;

    #endregion

    #region snippet_Constructors

    public PhotoRepositoryTests()
        => _contextOptions = new DbContextOptionsBuilder<EscortBookCustomerProfileContext>()
            .UseNpgsql(PostgresDatabase.CustomerProfile)
            .Options;

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return an empty list")]
    public async Task GetAllAsyncShouldReturnEmptyList()
    {
        using var context = new EscortBookCustomerProfileContext(_contextOptions);
        var photoRepository = new PhotoRepository(context);

        IEnumerable<Photo> photos = await photoRepository.GetAllAsync(p => p.ID == "63897195fce889036f476445", 1, 10);

        Assert.Empty(photos);
    }

    [Fact(DisplayName = "Should return null when record does not exists")]
    public async Task GetAsyncShouldReturnNull()
    {
        using var context = new EscortBookCustomerProfileContext(_contextOptions);
        var photoRepository = new PhotoRepository(context);

        Photo photo = await photoRepository.GetAsync(p => p.ID == "63897195fce889036f476445");

        Assert.Null(photo);
    }

    [Fact(DisplayName = "Should return 0 rows")]
    public async Task CountAsyncShouldReturn0()
    {
        using var context = new EscortBookCustomerProfileContext(_contextOptions);
        var photoRepository = new PhotoRepository(context);

        int counter = await photoRepository.CountAsync(p => p.ID == "63897195fce889036f476445");

        Assert.Equal<int>(0, counter);
    }

    [Fact(DisplayName = "Should create a new photo")]
    public async Task CreateAsyncShouldCreatePhoto()
    {
        using var context = new EscortBookCustomerProfileContext(_contextOptions);
        var photoRepository = new PhotoRepository(context);
        var photo = new Photo
        {
            Path = "profile.png",
            CustomerID = "63897a2d58a5da87baec34b1"
        };
        var profileRepository = new ProfileRepository(context);
        var profile = new Profile
        {
            CustomerID = "63897a2d58a5da87baec34b1",
            Email = "another.customer@example.com"
        };

        await profileRepository.CreateAsync(profile);
        await photoRepository.CreateAsync(photo);
        int counter = await photoRepository.CountAsync(p => p.CustomerID == "63897a2d58a5da87baec34b1");

        Assert.Equal<int>(1, counter);

        await photoRepository.DeleteAsync(p => p.CustomerID == "63897a2d58a5da87baec34b1");
        await profileRepository.DeleteAsync(p => p.CustomerID == "63897a2d58a5da87baec34b1");
    }

    [Fact(DisplayName = "Should delete an existing photo")]
    public async Task DeleteAsyncShouldDeletePhoto()
    {
        using var context = new EscortBookCustomerProfileContext(_contextOptions);
        var photoRepository = new PhotoRepository(context);
        var photo = new Photo
        {
            Path = "profile.png",
            CustomerID = "63897a2d58a5da87baec34b1"
        };
        var profileRepository = new ProfileRepository(context);
        var profile = new Profile
        {
            CustomerID = "63897a2d58a5da87baec34b1",
            Email = "another.customer@example.com"
        };

        await profileRepository.CreateAsync(profile);
        await photoRepository.CreateAsync(photo);
        int counterBeforeDelete = await photoRepository.CountAsync(p => p.CustomerID == "63897a2d58a5da87baec34b1");

        Assert.Equal<int>(1, counterBeforeDelete);

        await photoRepository.DeleteAsync(p => p.CustomerID == "63897a2d58a5da87baec34b1");
        int counterAfterDelete = await photoRepository.CountAsync(p => p.CustomerID == "63897a2d58a5da87baec34b1");

        Assert.Equal<int>(0, counterAfterDelete);

        await profileRepository.DeleteAsync(p => p.CustomerID == "63897a2d58a5da87baec34b1");
    }

    #endregion
}
