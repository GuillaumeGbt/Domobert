using Blazor_Domobert.Models;
using Blazor_Domobert.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Text.Json;

namespace Blazor_Domobert.Pages.Widget
{
    public partial class Light
    {
        [Parameter]
        public Device Device { get; set; }

        [Inject]
        public SignalRService _signalRService { get; set; }

        private bool isLightOn;

        protected override async Task OnInitializedAsync()
        {
            await _signalRService.ConnectAsync(Device.TopicMQTT, HandleNotification);
        }

        private void ToggleLamp()
        {
            isLightOn = !isLightOn;
        }

        private void HandleNotification(string message)
        {
            isLightOn = JsonSerializer.Deserialize<bool>(message);
            StateHasChanged(); // Met à jour l'UI
        }

        public async ValueTask DisposeAsync()
        {
            _signalRService.OnReceiveNotification -= HandleNotification;
            await _signalRService.DisconnectAsync();
        }
    }
}

