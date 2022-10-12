using System.Threading;
using System.Threading.Tasks;
using EscortBookCustomerProfile.Handlers;
using EscortBookCustomerProfile.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EscortBookCustomerProfile.Backgrounds;

public class S3Consumer : BackgroundService
{
    #region snippet_Properties

    private readonly IOperationHandler<string> _operationHandler;

    private readonly IAWSS3Service _s3Service;

    private readonly ILogger _logger;

    #endregion

    #region snippet_Constructors

    public S3Consumer
    (
        IOperationHandler<string> operationHandler,
        IServiceScopeFactory factory,
        ILogger<S3Consumer> logger
    )
    {
        _operationHandler = operationHandler;
        _s3Service = factory
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<IAWSS3Service>();
        _logger = logger;
    }

    #endregion

    #region snippet_ActionMethods

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _operationHandler.Subscribe("S3Consumer", async key =>
        {
            await _s3Service.DeleteObjectAsync(key);
            _logger.LogInformation("SUCCESSFUL DELETE FILE ON S3");
        });
        return Task.CompletedTask;
    }

    #endregion
}
