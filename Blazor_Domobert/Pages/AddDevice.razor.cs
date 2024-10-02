using Blazor_Domobert.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text.Json;

namespace Blazor_Domobert.Pages
{
    public partial class AddDevice
    {
        public DeviceAdd FormAddDevice { get; set; }

        [Inject]
        public HttpClient Client { get; set; }

        private string url = "https://localhost:7094/api/Devices";

        public AddDevice()
        {
            FormAddDevice = new DeviceAdd();
        }


        protected override async Task OnInitializedAsync()
        {
            Client.BaseAddress = new Uri(url);
        }

        public void OnSubmit()
        {
            Console.WriteLine(JsonSerializer.Serialize(FormAddDevice));
            var response = Client.PostAsJsonAsync<DeviceAdd>(url, FormAddDevice);
            if (response.Result.IsSuccessStatusCode) 
            {
                
            }
        }
    }
}
