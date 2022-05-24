using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace EscortBookCustomerProfile.Services
{
    public class KafkaService : IKafkaService
    {
        #region snippet_Properties

        private readonly IConfiguration _configuration;

        #endregion

        #region snippet_Constructors

        public KafkaService(IConfiguration configuration)
            => (_configuration) = (configuration);

        #endregion

        #region snippet_ActionMethods

        public async Task SendMessageAsync(string topic, Message<Null, string> message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _configuration["Kafka:Servers"],
                ClientId = _configuration["Kafka:ClientId"]
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            await producer.ProduceAsync(topic, message);
        }

        #endregion
    }
}
