using Blazor_Domobert.Models;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Net.Http.Json;
using System.Text.Json;

namespace Blazor_Domobert.Pages
{
    public partial class AddDevice
    {
        public DeviceAdd FormAddDevice { get; set; }

        [Inject]
        public HttpClient Client { get; set; }

        [Inject]
        public NotificationService NotificationService { get; set; }

        private string url = "https://localhost:7094/api/Devices";

        public AddDevice()
        {
            FormAddDevice = new DeviceAdd();
        }

        protected override async Task OnInitializedAsync()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(url);
        }

        public async Task OnSubmit()
        {
            try
            {
                Console.WriteLine(JsonSerializer.Serialize(FormAddDevice));
                var response = await Client.PostAsJsonAsync<DeviceAdd>(url, FormAddDevice);

                // Si la réponse est un succès
                if (response.IsSuccessStatusCode)
                {
                    ShowToast("Success", "Device added successfully!", NotificationSeverity.Success);
                    FormAddDevice = new DeviceAdd();
                }
                else
                {
                    ShowToast("Error", "Failed to add device.", NotificationSeverity.Error);
                }
            }
            catch (Exception ex)
            {
                ShowToast("Error", $"An error occurred: {ex.Message}", NotificationSeverity.Error);
            }
        }

        private void ShowToast(string summary, string detail, NotificationSeverity severity)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = severity,
                Summary = summary,
                Detail = detail,
                Duration = 4000
            });
        }
    }
}