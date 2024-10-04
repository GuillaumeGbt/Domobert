using Common;
using Dal = DAL.Services;
using MQTTnet;
using MQTTnet.Client;
using System.Text;

namespace BLL.Services
{
    public class MqttService
    {
        private readonly Dal.MqttService _mqttService;

        public MqttService(Dal.MqttService mqttService)
        {
            _mqttService = mqttService;
        }

        public async Task PublishMessage(string topic, string message)
        {
            await _mqttService.PublishAsync(topic, message);
        }

        public async Task SubscribeToTopic(string topic)
        {
            throw new NotImplementedException();
        }
    }
}
