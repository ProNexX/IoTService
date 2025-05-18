using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace IoT.BlazorClient.Services
{
    public class TelemetryService
    {
        private HubConnection _hubConnection = null!;
        private readonly string _hubUrl = "https://localhost:7041/TelemetryHub";
        private bool _isConnecting = false;

        public event Action<string> OnTelemetryReceived = null!;
        public event Action<string> OnConnectionError = null!;

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        public async Task StartAsync()
        {
            if (_isConnecting) return;
            _isConnecting = true;

            try
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(_hubUrl)
                    .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10) })
                    .Build();

                _hubConnection.On<string>("ReceiveTelemetry", (message) =>
                {
                    OnTelemetryReceived?.Invoke(message);
                });

                _hubConnection.Closed += async (error) =>
                {
                    if (error != null)
                    {
                        OnConnectionError?.Invoke($"Connection closed due to error: {error.Message}");
                    }
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    await TryConnectAsync();
                };

                await TryConnectAsync();
            }
            finally
            {
                _isConnecting = false;
            }
        }

        private async Task TryConnectAsync()
        {
            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("Connected to SignalR hub");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to SignalR hub: {ex.Message}");
                OnConnectionError?.Invoke($"Cannot connect to server at {_hubUrl}. Please ensure the backend server is running.");
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}