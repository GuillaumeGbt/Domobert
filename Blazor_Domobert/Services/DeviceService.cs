using Blazor_Domobert.Models;
using System.Net.Http.Json;

namespace Blazor_Domobert.Services
{
    public class DeviceService
    {
        private readonly HttpClient _httpClient;

        public DeviceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Device>> GetSensorsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Device>>("https://localhost:7094/api/Devices");
            return response ?? new List<Device>();
        }
    }
}
