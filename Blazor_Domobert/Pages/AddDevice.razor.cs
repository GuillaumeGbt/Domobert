using Blazor_Domobert.Models;
using Blazor_Domobert.Pages.Widget;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;
using System.Xml.Linq;

namespace Blazor_Domobert.Pages
{
    public partial class AddDevice
    {
        public DeviceAdd FormAddDevice { get; set; }

        internal class Type
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        string? value;
        List<Type> Types = new List<Type> 
        {
            new Type{Value= "Light",Name="Lampe" },
            new Type{Value= "Heater",Name="Chauffage" },
            new Type{Value= "Camera",Name="Caméra" },
            new Type{Value= "Alarm",Name="Alarme" },
            new Type{Value= "Temphumi",Name="Température et humidité" },
            new Type{Value= "Solar",Name="Panneau solaire" }
        };

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
                    var errorMessage = await response.Content.ReadAsStringAsync(); // Lire le contenu en tant que chaîne
                    ShowToast("Error", "Failed to add device.\n" + errorMessage, NotificationSeverity.Error);
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
                Duration = 8000
            });
        }
    }
}