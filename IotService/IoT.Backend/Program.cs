using IoT.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Only add Kafka consumer if Kafka is running
// For testing, you can comment this out if Kafka isn't available
builder.Services.AddHostedService<TelemetryConsumerService>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS with more detailed policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        //policy.WithOrigins("https://localhost:7256")
        //      .AllowAnyHeader()
        //      .AllowAnyMethod()
        //      .AllowCredentials();

        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure SignalR with detailed logging
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

// Apply CORS before routing
app.UseCors("AllowBlazorClient");

app.UseRouting();
app.UseAuthorization();

// Map endpoints after CORS and routing are set up
app.MapControllers();
app.MapHub<TelemetryHub>("/telemetryHub");

// Log startup information
app.Lifetime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Application started. SignalR hub available at: https://localhost:7041/telemetryHub");
});

app.Run();