using System.Threading.Tasks;
using Confluent.Kafka;

namespace EscortBookCustomerProfile.Web.Services;

public interface IKafkaService
{
    Task SendMessageAsync(string topic, Message<Null, string> message);
}
