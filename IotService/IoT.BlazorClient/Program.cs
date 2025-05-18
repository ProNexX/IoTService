using IoT.BlazorClient;
using IoT.BlazorClient.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Set the backend API base address
var backendUrl = "https://localhost:7041";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(backendUrl) });

// Register TelemetryService as a singleton so it persists between page navigations
builder.Services.AddSingleton<TelemetryService>();

builder.Services.AddMudServices();

// Add logging for debugging
builder.Logging.SetMinimumLevel(LogLevel.Debug);

await builder.Build().RunAsync();