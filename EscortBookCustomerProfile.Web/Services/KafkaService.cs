using System.Threading.Tasks;
using Confluent.Kafka;

namespace EscortBookCustomerProfile.Web.Services;

public class KafkaService : IKafkaService
{
    #region snippet_Properties

    private readonly IProducer<Null, string> _producer;

    #endregion

    #region snippet_Constructors

    public KafkaService(IProducer<Null, string> producer)
        => _producer = producer;

    #endregion

    #region snippet_ActionMethods

    public async Task SendMessageAsync(string topic, Message<Null, string> message)
        => await _producer.ProduceAsync(topic, message);

    #endregion
}
