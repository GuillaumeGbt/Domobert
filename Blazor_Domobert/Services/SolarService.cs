
using Blazor_Domobert.Models.SignalR;
using System.Net.Http.Json;

namespace Blazor_Domobert.Services
{
    public class SolarService
    {
        private readonly HttpClient _httpClient;

        public SolarService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SolarData>> GetTempHumiAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<List<SolarData>>("https://localhost:7094/api/MqttGlobal/" + id);
            return response ?? new List<SolarData>();
        }
    }
}

