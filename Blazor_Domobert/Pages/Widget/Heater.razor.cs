using Blazor_Domobert.Models;
using Blazor_Domobert.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Text.Json;

namespace Blazor_Domobert.Pages.Widget
{
    public partial class Heater
    {
        [Parameter]
        public Device Device { get; set; }
        [Inject]
        public SignalRService _signalRService { get; set; }

        private double targetTemperature = 15;
        protected override async Task OnInitializedAsync()
        {
            await _signalRService.ConnectAsync(Device.TopicMQTT, HandleNotification);
        }

        private void SetTemperature()
        {
            Console.WriteLine($"Température définie : {targetTemperature}°C");
        }

        private void HandleNotification(string message)
        {
            targetTemperature = JsonSerializer.Deserialize<double>(message);
            targetTemperature = targetTemperature < 15 ? 15 : targetTemperature;
            StateHasChanged(); // Met à jour l'UI
        }

        public async ValueTask DisposeAsync()
        {
            _signalRService.OnReceiveNotification -= HandleNotification;
            await _signalRService.DisconnectAsync();
        }


    }
}
