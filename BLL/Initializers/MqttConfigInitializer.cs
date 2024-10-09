using BLL.Models.MQTT;
using BLL.Services;
using Common;
using DAL.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Initializers
{
    public class MqttConfigInitializer
    {
        private readonly MqttConfigService _mqttConfigService;
        private readonly IServiceProvider _serviceProvider;

        public MqttConfigInitializer(MqttConfigService mqttConfigService, IServiceProvider serviceProvider)
        {
            _mqttConfigService = mqttConfigService;
            _serviceProvider = serviceProvider;
        }

        public void InitializeConfig()
        {
            // Utilise _serviceProvider pour obtenir une instance de TopicDbService (scoped)
            using (var scope = _serviceProvider.CreateScope())
            {
                var deviceService = scope.ServiceProvider.GetRequiredService<IDeviceRepository<Device>>();

                // Récupérer les topics de la base de données
                List<TypeAndTopic> dbConfig= deviceService.GetAll()
                                                    .Select(t => new TypeAndTopic { Type = t.Type.ToEDeviceType(), Topic = t.TopicMQTT })
                                                    .ToList();

                // Charger la configuration actuelle du fichier YAML
                List<TypeAndTopic> currentConfig = _mqttConfigService.LoadConfig();

                // Comparer les deux et effectuer les ajouts ou suppressions si nécessaire
                var topicsToAdd = dbConfig.Except(currentConfig, new TypeAndTopicComparer()).ToList();
                var topicsToRemove = currentConfig.Except(dbConfig, new TypeAndTopicComparer()).ToList();

                // Ajouter les nouveaux topics
                foreach (var topic in topicsToAdd)
                {
                    _mqttConfigService.AddTopic(topic);
                }

                // Supprimer les anciens topics
                foreach (var topic in topicsToRemove)
                {
                    _mqttConfigService.RemoveTopic(topic);
                }
            }
        }
    }

    public class TypeAndTopicComparer : IEqualityComparer<TypeAndTopic>
    {
        public bool Equals(TypeAndTopic x, TypeAndTopic y)
        {
            // Comparer les types et les topics
            return x.Type == y.Type && x.Topic == y.Topic;
        }

        public int GetHashCode(TypeAndTopic obj)
        {
            return (obj.Type + obj.Topic).GetHashCode();
        }
    }

}
