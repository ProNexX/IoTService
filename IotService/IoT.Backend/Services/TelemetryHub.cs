using Microsoft.AspNetCore.SignalR;

namespace IoT.Backend.Services
{
    public class TelemetryHub : Hub
    {
        // This method can be called from clients
        public async Task SendTelemetry(string message)
        {
            await Clients.All.SendAsync("ReceiveTelemetry", message);
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            await Clients.Caller.SendAsync("ReceiveTelemetry", "Connected to telemetry hub!");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}