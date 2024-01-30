using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace KafkaProducer.Controllers
{
    [ApiController]
    [Route("/api/[controller]/")]
    public class KafkaProducerController : ControllerBase
    {
        private readonly ILogger<KafkaProducerController> _logger;
        private readonly IProducer<Null, string> _producer;

        public KafkaProducerController(ILogger<KafkaProducerController> logger)
        {
            _logger = logger;
            var config = new ProducerConfig
            {
                BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_ADDRESS")

            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string message, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Sending >> {message}");

            await _producer.ProduceAsync(
                "demo-topic",
                new Message<Null, string> { Value = message },
                cancellationToken);
            return Ok();
        }
    }
}
