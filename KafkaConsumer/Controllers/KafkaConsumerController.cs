using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace KafkaConsumer.Controllers
{
    [ApiController, Route("/api/[controller]/")]
    public class KafkaConsumerController : ControllerBase
    {
        private readonly ILogger<KafkaConsumerController> _logger;
        private readonly IConsumer<Null, string> _consumer;

        public KafkaConsumerController(ILogger<KafkaConsumerController> logger)
        {
            _logger = logger;
            var config = new ConsumerConfig
            {
                BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_ADDRESS"),
                GroupId = "demo-group",
                AutoOffsetReset = AutoOffsetReset.Latest
            };
            _consumer = new ConsumerBuilder<Null, string>(config).Build();
        }

        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            _consumer.Subscribe("demo-topic");
            string? message = null;
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(cancellationToken);
                _logger.LogInformation("Getting >> {Message}", result.Message.Value);
                message += result.Message.Value + "\r\n";
            }            
            return Ok(message);
        }

    }
}
