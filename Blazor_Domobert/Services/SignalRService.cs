using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Blazor_Domobert.Services
{
    public class SignalRService
    {
        private readonly NavigationManager _navigationManager;
        private HubConnection _hubConnection;

        public event Action<string> OnReceiveNotification;

        public SignalRService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public async Task ConnectAsync(string topic, Action<string> onNotification)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_navigationManager.ToAbsoluteUri("https://localhost:7094/deviceHub"))
                .Build();

            _hubConnection.On<string>(topic, (message) =>
            {
                onNotification?.Invoke(message);
            });

            await _hubConnection.StartAsync();
        }

        public async Task DisconnectAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
            }
        }
    }
}
