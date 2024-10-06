using DAL.Models;
using DAL.Services;
using Common;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace BLL.BackgroundServices
{
    public class MqttBackgroundService(IDeviceRepository<Device> deviceRepository, MqttService mqttService) : BackgroundService
    {
        public class TypeAndTopic
        {
            public eDeviceType Type { get; set; }
            public string Topic { get; set; }
        }

        //private readonly MqttService _mqttService;
        //private readonly IDeviceRepository<Device> _deviceRepository;
        private List<TypeAndTopic> _typesAndTopics;

        //public MqttBackgroundService()
        //{
        //    //_mqttService = mqttService;
        //    //_deviceRepository = deviceRepository;

        //    // Charger les topics des devices dans la liste au démarrage
        //}

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _typesAndTopics = deviceRepository.GetAll()
                .Select(e => new TypeAndTopic { Topic = e.TopicMQTT, Type = e.Type.ToEDeviceType() })
                .ToList();
            //Abonnement aux topics lorsque le service démarre
            foreach (var typeAndTopic in _typesAndTopics)
            {
                SubscribeToTopic(typeAndTopic);
            }

            return Task.CompletedTask;
        }

        private Task SubscribeToTopic(TypeAndTopic typeAndTopic)
        {
            switch (typeAndTopic.Type)
            {
                case eDeviceType.TEMP_HUMI:
                    mqttService.SubscribeAsync(typeAndTopic.Topic, HandleTempHumi);
                    break;
                case eDeviceType.LIGHT:
                    mqttService.SubscribeAsync(typeAndTopic.Topic, HandleLight);
                    break;
                case eDeviceType.SOLAR:
                    mqttService.SubscribeAsync(typeAndTopic.Topic, HandleSolar);
                    break;
                case eDeviceType.CAMERA:
                    mqttService.SubscribeAsync(typeAndTopic.Topic, HandleCamera);
                    break;
                case eDeviceType.HEATER:
                    mqttService.SubscribeAsync(typeAndTopic.Topic, HandleHeater);
                    break;
                case eDeviceType.ALARM:
                    mqttService.SubscribeAsync(typeAndTopic.Topic, HandleAlarm);
                    break;
                default:
                    Console.WriteLine("Capteur non reconnu.");
                    break;
            }
            return Task.CompletedTask;
        }

        // Méthode pour traiter les messages
        private string ProcessMessage(string message)
        {
            try
            {
                var sensorMessage = JsonSerializer.Deserialize<string>(message);
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

        // Méthodes spécifiques pour traiter les types de capteurs
        private void HandleTempHumi(string message)
        {
            var sensorMessage = ProcessMessage(message);
            Console.WriteLine($"TempHumi: {sensorMessage}");
        }

        private void HandleLight(string message)
        {
            var sensorMessage = ProcessMessage(message);
            Console.WriteLine($"Light: {sensorMessage}");
        }

        private void HandleSolar(string message)
        {
            var sensorMessage = ProcessMessage(message);
            Console.WriteLine($"Solar: {sensorMessage}");
        }

        private void HandleCamera(string message)
        {
            var sensorMessage = ProcessMessage(message);
            Console.WriteLine($"Camera: {sensorMessage}");
        }

        private void HandleAlarm(string message)
        {
            var sensorMessage = ProcessMessage(message);
            Console.WriteLine($"Alarm: {sensorMessage}");
        }

        private void HandleHeater(string message)
        {
            var sensorMessage = ProcessMessage(message);
            Console.WriteLine($"Heater: {sensorMessage}");
        }
    }
}
