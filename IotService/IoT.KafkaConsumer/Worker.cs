using Confluent.Kafka;
using IoT.DataAccess.Models;
using System.Text.Json;

namespace IoT.KafkaConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "telemetry-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("telemetry-topic");

            _logger.LogInformation("Kafka Consumer started.");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);
                        Console.WriteLine(consumeResult.Message.Value);

                        var telemetryData = JsonSerializer.Deserialize<TelemetryData>(consumeResult.Message.Value);
                        //var telemetryData = new TelemetryData
                        //{
                        //    DeviceId = telemetryDataProto.DeviceId,
                        //    Timestamp = DateTime.Parse(telemetryDataProto.Timestamp).ToUniversalTime(),
                        //    Temperature = telemetryDataProto.Temperature,
                        //    Humidity = telemetryDataProto.Humidity
                        //};

                        if (telemetryData != null)
                        {
                            using var scope = _scopeFactory.CreateScope();
                            var dbContext = scope.ServiceProvider.GetRequiredService<TelemetryContext>();

                            dbContext.TelemetryRecords.Add(telemetryData);
                            await dbContext.SaveChangesAsync(stoppingToken);

                            _logger.LogInformation($"Stored telemetry data: {telemetryData.DeviceId} at {telemetryData.Timestamp}");
                        }
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Error occurred: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Kafka Consumer stopped.");
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}
