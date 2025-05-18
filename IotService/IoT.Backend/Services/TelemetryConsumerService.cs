using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;

namespace IoT.Backend.Services
{
    public class TelemetryConsumerService : BackgroundService
    {
        private readonly ILogger<TelemetryConsumerService> _logger;
        private readonly IHubContext<TelemetryHub> _hubContext;
        private readonly string _bootstrapServers = "localhost:9092";
        private readonly string _topic = "telemetry-topic";

        public TelemetryConsumerService(ILogger<TelemetryConsumerService> logger, IHubContext<TelemetryHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting Kafka background service...");

            await Task.Run(() =>
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = _bootstrapServers,
                    GroupId = "blazor-client-group",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
                consumer.Subscribe(_topic);
                _logger.LogInformation("Kafka consumer subscribed to topic: {Topic}", _topic);

                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var result = consumer.Consume(stoppingToken);
                            _logger.LogInformation("Consumed message: {Message}", result.Message.Value);
                            _hubContext.Clients.All.SendAsync("ReceiveTelemetry", result.Message.Value);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Consume error");
                        }
                    }
                }
                finally
                {
                    consumer.Close();
                    _logger.LogInformation("Kafka consumer closed.");
                }
            }, stoppingToken);
        }
    }
}
