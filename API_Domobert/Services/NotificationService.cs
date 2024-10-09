using API_Domobert.Hubs;
using BLL.Services;
using Microsoft.AspNetCore.SignalR;

namespace API_Domobert.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<DeviceHub> _hubContext;

        public NotificationService(IHubContext<DeviceHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyClients(string topic, string message)
        {
            await _hubContext.Clients.All.SendAsync(topic, message);
        }
    }
}
