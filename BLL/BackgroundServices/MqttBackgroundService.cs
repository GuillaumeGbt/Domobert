using Dal = DAL.Services;
using Bll = BLL.Services;
using Common;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using BLL.Models.MQTT;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using BLL.Models.History;
using DAL.Services;

namespace BLL.BackgroundServices
{
    public class MqttBackgroundService(
        Dal.MqttService mqttService,
        Bll.MqttConfigService mqttConfigService,
        IServiceProvider serviceProvider)
        : BackgroundService
    {
        private List<TypeAndTopic> _typesAndTopics;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                var historyTempService = scope.ServiceProvider.GetRequiredService<Bll.HistoryTempService>();

                _typesAndTopics = mqttConfigService.LoadConfig();

                // Abonnement aux topics
                mqttService.SubscribeToTopicsAsync(_typesAndTopics.Select(t => t.Topic).ToArray()).Wait();

                // Abonnement aux topics lorsque le service démarre
                foreach (var typeAndTopic in _typesAndTopics)
                {
                    SubscribeToTopic(typeAndTopic, notificationService, historyTempService);  // Passe notificationService ici
                }
            }
            return Task.CompletedTask;
        }

        private Task SubscribeToTopic(
            TypeAndTopic typeAndTopic, 
            INotificationService notificationService,
            Bll.HistoryTempService historyTempService)
        {
            // Handler pour chaque type de capteur
            switch (typeAndTopic.Type)
            {
                case eDeviceType.TEMP_HUMI:
                    mqttService.SubscribeAsync(typeAndTopic.Topic, message => HandleTempHumi(message, notificationService, historyTempService,typeAndTopic.Topic));
                    break;
                case eDeviceType.LIGHT:
                    mqttService.SubscribeAsync(typeAndTopic.Topic, message => HandleLight(message, notificationService, typeAndTopic.Topic));
                    break;
                case eDeviceType.SOLAR:
                    mqttService.SubscribeAsync(typeAndTopic.Topic, message => HandleSolar(message, notificationService, typeAndTopic.Topic));
                    break;
                case eDeviceType.CAMERA:
                    mqttService.SubscribeAsync(typeAndTopic.Topic, message => HandleCamera(message, notificationService, typeAndTopic.Topic));
                    break;
                case eDeviceType.HEATER:
                    mqttService.SubscribeAsync(typeAndTopic.Topic, message => HandleHeater(message, notificationService, typeAndTopic.Topic));
                    break;
                case eDeviceType.ALARM:
                    mqttService.SubscribeAsync(typeAndTopic.Topic, message => HandleAlarm(message, notificationService, typeAndTopic.Topic));
                    break;
                default:
                    Console.WriteLine("Capteur non reconnu.");
                    break;
            }
            return Task.CompletedTask;
        }

        // Méthode pour traiter les messages
        private T ProcessMessage<T>(string message)
        {
            try
            {
                var sensorMessage = JsonSerializer.Deserialize<T>(message);
                if (sensorMessage == null)
                {
                    throw new Exception("Message non valide reçu.");
                }

                return sensorMessage;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erreur de désérialisation: {ex.Message}");
                throw;
            }
        }

        private void HandleTempHumi(
            string message, 
            INotificationService notificationService, 
            Bll.HistoryTempService historyTempService, 
            string topic)
        {
            TempHumi sensorMessage = ProcessMessage<TempHumi>(message);

            TempHumiData sensorData = new TempHumiData()
            {
                Temperature = sensorMessage.temperature,
                Humidity = sensorMessage.humidity,
                Timestamp = DateTime.Now,
                Topic = topic
            };
            historyTempService.Add(sensorData);


            notificationService.NotifyClients(topic, message).Wait();
        }

        private void HandleLight(string message, INotificationService notificationService, string topic)
        {
            notificationService.NotifyClients(topic, message).Wait();
        }

        private void HandleSolar(string message, INotificationService notificationService, string topic)
        {
            var sensorMessage = ProcessMessage<Solar>(message);

            notificationService.NotifyClients(topic, message).Wait();

        }

        private void HandleCamera(string message, INotificationService notificationService, string topic)
        {

            notificationService.NotifyClients(topic, message).Wait();

        }

        private void HandleAlarm(string message, INotificationService notificationService, string topic)
        {

            notificationService.NotifyClients(topic, message).Wait();

        }

        private void HandleHeater(string message, INotificationService notificationService, string topic)
        {
            Console.WriteLine($"Heater: {message}");
            notificationService.NotifyClients(topic, message).Wait();

        }
    }
}
