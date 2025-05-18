using IoT.GrpcService.Services;
using IoT.KafkaProducer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<KafkaProducerService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
    var topic = configuration["Kafka:Topic"] ?? "telemetry-topic";
    return new KafkaProducerService(bootstrapServers, topic);
});

builder.Services.AddGrpc();

var app = builder.Build();

app.UseGrpcWeb();
app.MapGrpcService<TelemetryServiceImpl>().EnableGrpcWeb();

app.Run();
