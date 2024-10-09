using BLL.Models.MQTT;
using Common;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BLL.Services
{
    public class MqttConfigService
    {
        private readonly string _filePath = "MQTT_Topic.yaml";
        private readonly IDeserializer _deserializer;
        private readonly ISerializer _serializer;

        public MqttConfigService()
        {
            _deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            _serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
        }

        public List<TypeAndTopic> LoadConfig()
        {
            if (!File.Exists(_filePath))
                return new List<TypeAndTopic>();

            var yaml = File.ReadAllText(_filePath);
            return _deserializer.Deserialize<List<TypeAndTopic>>(yaml);
        }

        public void SaveConfig(List<TypeAndTopic> config)
        {
            var yaml = _serializer.Serialize(config);
            File.WriteAllText(_filePath, yaml);
        }

        public void AddTopic(TypeAndTopic typeAndTopic)
        {
            var config = LoadConfig();
            config.Add(typeAndTopic);
            SaveConfig(config);
        }

        public void RemoveTopic(TypeAndTopic typeAndTopic)
        {
            var config = LoadConfig();
            config.Remove(typeAndTopic);
            SaveConfig(config);
        }
    }
}
