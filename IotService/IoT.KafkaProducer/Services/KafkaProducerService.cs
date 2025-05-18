using Confluent.Kafka;
using System.Text.Json;

namespace IoT.KafkaProducer.Services
{
    public class KafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaProducerService(string bootstrapServers, string topic)
        {
            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
            _producer = new ProducerBuilder<Null, string>(config).Build();
            _topic = topic;
        }

        public async Task ProduceAsync<T>(T message)
        {
            var jsonMessage = JsonSerializer.Serialize(message);
            await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = jsonMessage });
        }
    }
}
