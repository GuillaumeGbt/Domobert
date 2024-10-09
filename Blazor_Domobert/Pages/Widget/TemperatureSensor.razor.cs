using Blazor_Domobert.Models;
using live = Blazor_Domobert.Models.SignalR;
using Blazor_Domobert.Services;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace Blazor_Domobert.Pages.Widget
{
    public partial class TemperatureSensor
    {
        [Parameter]
        public Device Device { get; set; }

        [Inject]
        public SignalRService _signalRService { get; set; }
        [Inject]
        public TempHumiService _tempHumiService { get; set; }

        private List<TempHumiData> historyData = new();
        private live.TempHumiData currentData;

        protected override async Task OnInitializedAsync()
        {
            historyData = await _tempHumiService.GetTempHumiAsync(Device.Id);

            historyData = historyData
                .GroupBy(data => data.Timestamp.Date) // Grouper par jour
                .Select(g => new TempHumiData
                {
                    Timestamp = g.Key, // Date du jour
                    Temperature = g.Average(d => d.Temperature), // Moyenne de température
                    Humidity = g.Average(d => d.Humidity) // Moyenne d'humidité
                }).ToList();

            await _signalRService.ConnectAsync(Device.TopicMQTT, HandleNotification);
        }

            private void HandleNotification(string message)
        {
            currentData = JsonSerializer.Deserialize<Models.SignalR.TempHumiData>(message);
            StateHasChanged(); // Met à jour l'UI
        }

        public async ValueTask DisposeAsync()
        {
            _signalRService.OnReceiveNotification -= HandleNotification;
            await _signalRService.DisconnectAsync();
        }
    }
}
