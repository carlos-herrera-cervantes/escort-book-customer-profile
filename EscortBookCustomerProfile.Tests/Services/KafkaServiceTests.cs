using System.ComponentModel;
using Xunit;
using System.Diagnostics.CodeAnalysis;
using Moq;
using Confluent.Kafka;
using System.Threading.Tasks;
using System.Threading;
using EscortBookCustomerProfile.Web.Services;

namespace EscortBookCustomerProfile.Tests.Services;

[Category("Services")]
[Collection(nameof(KafkaService))]
[ExcludeFromCodeCoverage]
public class KafkaServiceTests
{
    #region snippet_Properties

    private readonly Mock<IProducer<Null, string>> _mockProducer;

    #endregion

    #region snippet_Constructors

    public KafkaServiceTests() => _mockProducer = new Mock<IProducer<Null, string>>();

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should successfully invoke ProduceAsync method of the service")]
    public async Task SendMessageAsyncShouldBeSuccessfully()
    {
        _mockProducer
            .Setup(x => x.ProduceAsync(
                It.IsAny<string>(),
                It.IsAny<Message<Null, string>>(),
                It.IsAny<CancellationToken>()
            ));

        var kafkaService = new KafkaService(_mockProducer.Object);

        await kafkaService.SendMessageAsync(topic: "test-topic", message: new Message<Null, string>());

        _mockProducer.Verify(x => x.ProduceAsync(
                It.IsAny<string>(),
                It.IsAny<Message<Null, string>>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
    }

    #endregion
}
