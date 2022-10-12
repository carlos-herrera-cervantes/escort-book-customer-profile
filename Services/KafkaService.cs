using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace EscortBookCustomerProfile.Services;

public class KafkaService : IKafkaService
{
    #region snippet_ActionMethods

    public async Task SendMessageAsync(string topic, Message<Null, string> message)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_SERVERS"),
            ClientId = Environment.GetEnvironmentVariable("KAFKA_CLIENT_ID")
        };

        using var producer = new ProducerBuilder<Null, string>(config).Build();
        await producer.ProduceAsync(topic, message);
    }

    #endregion
}
