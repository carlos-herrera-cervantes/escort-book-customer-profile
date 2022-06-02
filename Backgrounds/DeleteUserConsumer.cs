using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using EscortBookCustomerProfile.Handlers;
using EscortBookCustomerProfile.Services;
using EscortBookCustomerProfile.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Backgrounds
{
    public class DeleteUserConsumer : BackgroundService
    {
        #region snippet_Properties
        
        private readonly IOperationHandler<DeleteUserEvent> _operationHandler;

        private readonly IKafkaService _kafkaService;

        private readonly ILogger _logger;

        #endregion

        #region snippet_Constructors

        public DeleteUserConsumer
        (
            IOperationHandler<DeleteUserEvent> operationHandler,
            IServiceScopeFactory factory,
            ILogger<BlockUserConsumer> logger
        )
        {
            _operationHandler = operationHandler;
            _kafkaService = factory
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<IKafkaService>();
            _logger = logger;
        }
        
        #endregion

        #region snippet_ActionMethods

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _operationHandler.Subscribe("DeleteUserConsumer", async deleteUserEvent =>
            {
                var deleteUserEventStr = JsonConvert.SerializeObject(deleteUserEvent);
                var message = new Message<Null, string> {Value = deleteUserEventStr};
                await _kafkaService.SendMessageAsync("user-delete-account", message);

                _logger.LogInformation($"SUCCESSFUL PROPAGATED DELETION OF CUSTOMER");
            });

            return Task.CompletedTask;
        }

        #endregion
    }
}
