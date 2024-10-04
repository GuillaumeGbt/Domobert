using Blazor_Domobert.Models;
using Blazor_Domobert.Services;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Blazor_Domobert.Pages
{
    public partial class Dashboard
    {
        [Inject]
        public DeviceService DeviceService { get; set; }
        [Inject]
        public NotificationService NotificationService { get; set; }

        private List<Device> devices;
        private const int MaxRetryAttempts = 5;
        private const int RetryDelayMilliseconds = 10000;

        protected override async Task OnInitializedAsync()
        {
            int retryCount = 0;
            bool success = false;

            do
            {
                try
                {
                    devices = await DeviceService.GetSensorsAsync();

                    if (devices != null && devices.Any())
                    {
                        success = true;
                    }
                    else
                    {
                        throw new Exception("No devices found");
                    }
                }
                catch (Exception ex)
                {
                    retryCount++;

                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Error",
                        Detail = $"Attempt {retryCount} failed: {ex.Message}",
                        Duration = 8000
                    });

                    if (retryCount < MaxRetryAttempts)
                    {
                        await Task.Delay(RetryDelayMilliseconds);
                    }
                }

            } while (!success && retryCount < MaxRetryAttempts);

            if (!success)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = "Failed to retrieve devices after multiple attempts.",
                    Duration = 15000
                });
            }
        }
    }
}
