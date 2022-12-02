using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using System;
using EscortBookCustomerProfile.Web.Handlers;
using EscortBookCustomerProfile.Web.Models;
using EscortBookCustomerProfile.Web.Repositories;

namespace EscortBookCustomerProfile.Web.Backgrounds;

public class ProfileStatusConsumer : BackgroundService
{
    #region snippet_Properties

    private readonly IOperationHandler<Profile> _operationHandler;

    private readonly IProfileStatusRepository _profileStatusRepository;

    private readonly IProfileStatusCategoryRepository _profileStatusCategoryRepository;

    #endregion

    #region snippet_Constructors

    public ProfileStatusConsumer(IOperationHandler<Profile> operationHandler, IServiceScopeFactory factory)
    {
        var serviceProvider = factory.CreateScope().ServiceProvider;

        _operationHandler = operationHandler;
        _profileStatusRepository = serviceProvider.GetRequiredService<IProfileStatusRepository>();
        _profileStatusCategoryRepository = serviceProvider.GetRequiredService<IProfileStatusCategoryRepository>();
    }

    #endregion

    #region snippet_ActionMethods

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Func<Profile, Task> createProfileStatusFn = async (Profile profile) =>
        {
            var statusCategory = await _profileStatusCategoryRepository.GetAsync(c => c.Name == "Created");

            if (statusCategory is null) return;

            var newProfileStatus = new ProfileStatus
            {
                CustomerID = profile.CustomerID,
                ProfileStatusCategoryID = statusCategory.ID
            };

            await _profileStatusRepository.CreateAsync(newProfileStatus);
        };

        _operationHandler.Subscribe("ProfileStatusConsumer", async profile
            => await createProfileStatusFn(profile));

        return Task.CompletedTask;
    }

    #endregion
}
