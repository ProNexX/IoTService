using IoT.DataAccess.Models;
using IoT.KafkaConsumer;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<TelemetryContext>(options =>
                    options.UseNpgsql("Host=localhost;Database=telemetrydb;Username=postgres;Password=123456a."));

var host = builder.Build();
host.Run();
