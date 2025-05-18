using Grpc.Core;
using IoT.KafkaProducer.Services;
using IoTService;

namespace IoT.GrpcService.Services
{
    public class TelemetryServiceImpl : TelemetryService.TelemetryServiceBase
    {
        private readonly ILogger<TelemetryServiceImpl> _logger;
        private readonly KafkaProducerService _kafkaProducer;

        public TelemetryServiceImpl(ILogger<TelemetryServiceImpl> logger, KafkaProducerService kafkaProducer)
        {
            _logger = logger;
            _kafkaProducer = kafkaProducer;
        }

        public override async Task<TelemetryAck> SendTelemetry(TelemetryData request, ServerCallContext context)
        {
            _logger.LogInformation($"Received data from {request.DeviceId}: Temp={request.Temperature}, Humidity={request.Humidity}");

            // Publish to Kafka
            await _kafkaProducer.ProduceAsync(request);

            return new TelemetryAck { Status = "Received" };
        }
    }
}
