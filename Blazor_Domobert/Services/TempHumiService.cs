using Blazor_Domobert.Models;
using System.Net.Http.Json;

namespace Blazor_Domobert.Services
{
    public class TempHumiService
    {
        private readonly HttpClient _httpClient;

        public TempHumiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TempHumiData>> GetTempHumiAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<List<TempHumiData>>("https://localhost:7094/api/MqttGlobal/" + id);
            return response ?? new List<TempHumiData>();
        }
    }
}

