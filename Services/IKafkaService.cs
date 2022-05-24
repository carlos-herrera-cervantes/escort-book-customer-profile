using System.Threading.Tasks;
using Confluent.Kafka;

namespace EscortBookCustomerProfile.Services
{
    public interface IKafkaService
    {
        Task SendMessageAsync(string topic, Message<Null, string> message);
    }
}
