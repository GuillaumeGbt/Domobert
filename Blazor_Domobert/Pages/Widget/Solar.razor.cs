using Blazor_Domobert.Models;
using Blazor_Domobert.Models.SignalR;
using Blazor_Domobert.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Text.Json;

namespace Blazor_Domobert.Pages.Widget
{
    public partial class Solar
    {
        [Parameter]
        public Device Device { get; set; }

        [Inject]
        public SignalRService _signalRService { get; set; }
        [Inject]
        public SolarService _solarService { get; set; }

        private List<SolarData> historyData = new();
        private SolarData currentData;

        protected override async Task OnInitializedAsync()
        {
            historyData = await _solarService.GetTempHumiAsync(Device.Id);

            await _signalRService.ConnectAsync(Device.TopicMQTT, HandleNotification);
        }

        private void HandleNotification(string message)
        {
            currentData = JsonSerializer.Deserialize<SolarData>(message);
            StateHasChanged(); // Met à jour l'UI
        }

        public async ValueTask DisposeAsync()
        {
            _signalRService.OnReceiveNotification -= HandleNotification;
            await _signalRService.DisconnectAsync();
        }
    }
}

