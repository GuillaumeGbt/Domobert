using Microsoft.AspNetCore.SignalR;

namespace API_Domobert.Hubs
{
    public class DeviceHub: Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveSensorData", message);
        }
    }
}
