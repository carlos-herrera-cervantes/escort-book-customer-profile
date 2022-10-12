using EscortBookCustomerProfile.Handlers;
using EscortBookCustomerProfile.Models;
using EscortBookCustomerProfile.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Backgrounds;

public class ProfileStatusConsumer : BackgroundService
{
    #region snippet_Properties

    private readonly IOperationHandler<Profile> _operationHandler;

    private readonly IProfileStatusRepository _profileStatusRepository;

    private readonly IProfileStatusCategoryRepository _profileStatusCategoryRepository;

    #endregion

    #region snippet_Constructors

    public ProfileStatusConsumer
    (
        IOperationHandler<Profile> operationHandler,
        IServiceScopeFactory factory
    )
    {
        _operationHandler = operationHandler;
        _profileStatusRepository = factory
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<IProfileStatusRepository>();

        _profileStatusCategoryRepository = factory
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<IProfileStatusCategoryRepository>();
    }

    #endregion

    #region snippet_ActionMethods

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _operationHandler.Subscribe("ProfileStatusConsumer", async profile =>
        {
            var statusCategory = await _profileStatusCategoryRepository.GetByName("Created");

            if (statusCategory is null) return;

            var newProfileStatus = new ProfileStatus
            {
                CustomerID = profile.CustomerID,
                ProfileStatusCategoryID = statusCategory.ID
            };

            await _profileStatusRepository.CreateAsync(newProfileStatus);
        });

        return Task.CompletedTask;
    }

    #endregion
}
