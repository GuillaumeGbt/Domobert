using Microsoft.AspNetCore.SignalR;

namespace BLL.Services
{
    public interface INotificationService
    {
        Task NotifyClients(string topic,string message);
    }
}