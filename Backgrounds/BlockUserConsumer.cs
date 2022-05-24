using EscortBookCustomerProfile.Handlers;
using EscortBookCustomerProfile.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using EscortBookCustomerProfile.Types;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EscortBookCustomerProfile.Backgrounds
{
    public class BlockUserConsumer : BackgroundService
    {
        #region snippet_Properties
        
        private readonly IOperationHandler<BlockUserEvent> _operationHandler;

        private readonly IKafkaService _kafkaService;

        private readonly ILogger _logger;

        #endregion
        
        #region snippet_Constructors

        public BlockUserConsumer
        (
            IOperationHandler<BlockUserEvent> operationHandler,
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
            _operationHandler.Subscribe("BlockUserConsumer", async blockUserEvent =>
            {
                var blockUserEventStr = JsonConvert.SerializeObject(blockUserEvent);
                var message = new Message<Null, string> {Value = blockUserEventStr};
                await _kafkaService.SendMessageAsync("block-user", message);

                _logger.LogInformation($"SUCCESSFUL PROPAGATED PROFILE STATUS");
            });

            return Task.CompletedTask;
        }

        #endregion
    }
}
