using Dal = DAL.Services;

namespace BLL.Services
{
    public class MqttService
    {
        private readonly Dal.MqttService _mqttService;

        public MqttService(Dal.MqttService mqttService)
        {
            _mqttService = mqttService;
        }

        public async Task SubscribeAsync(string topic, Action<string> callback)
        {
            _mqttService.SubscribeAsync(topic, callback);
            await _mqttService.SubscribeToTopicsAsync(new[] { topic });
        }

        public async Task<bool> PublishAsync(string topic, string message)
        {
            return await _mqttService.PublishAsync(topic, message);
        }
    }
}
