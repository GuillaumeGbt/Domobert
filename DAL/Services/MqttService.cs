using Common;
using DAL.Models;
using MQTTnet;
using MQTTnet.Client;
using System.Text;

namespace DAL.Services
{
    public class MqttService
    {
        public class Configuration
        {
            public string Host { get; set; } = string.Empty;
            public int Port { get; set; }
        }

        private readonly IMqttClient _client;

        public MqttService(Configuration configuration)
        {
            // Initialisation de la connexion au broker MQTT (Mosquitto)
            MqttFactory factory = new MqttFactory();

            // Définir le serveur et le port sur lequel se connecter
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(configuration.Host, configuration.Port)
                .WithCleanSession(false)  // Garde la session MQTT entre les connexions
                .Build();

            _client = factory.CreateMqttClient();

            // Tentative de connexion au broker
            ConnectAsync(options).Wait();

            // Gestion de la reconnexion
            _client.DisconnectedAsync += async (args) =>
            {
                int retries = 0;
                while (!_client.IsConnected && retries < 5)
                {
                    try
                    {
                        Console.WriteLine($"Attempting to reconnect ({retries + 1})...");
                        await _client.ReconnectAsync();
                        retries++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Reconnection attempt {retries} failed: {ex.Message}");
                        await Task.Delay(2000 * retries); // Délai progressif
                    }
                }
            };
        }

        // Connexion au broker MQTT
        public async Task<bool> ConnectAsync(MqttClientOptions options)
        {
            try
            {
                await _client.ConnectAsync(options);
                Console.WriteLine("Connected to MQTT broker.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to MQTT broker: {ex.Message}");
                return false;
            }
        }

        // Abonnement aux topics
        public async Task SubscribeToTopicsAsync(string[] topics)
        {
            var optionsBuilder = new MqttClientSubscribeOptionsBuilder();

            foreach (var topic in topics)
            {
                optionsBuilder.WithTopicFilter(f => f.WithTopic(topic));
            }

            try
            {
                var response = await _client.SubscribeAsync(optionsBuilder.Build());
                if (response.Items.All(i => i.ResultCode == MqttClientSubscribeResultCode.GrantedQoS0))
                {
                    Console.WriteLine("Successfully subscribed to all topics.");
                }
                else
                {
                    Console.WriteLine("Subscription failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error subscribing to topics: {ex.Message}");
            }
        }


        // Souscription à un topic avec un callback pour traiter le message
        public void SubscribeAsync(string topic, Action<string> callBack)
        {
            _client.ApplicationMessageReceivedAsync += async (e) =>
            {
                if (e.ApplicationMessage.Topic == topic)
                {
                    var message = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                    Task.Run(() => callBack?.Invoke(message)); // Callback exécuté sur un thread séparé
                }
                await Task.CompletedTask;
            };
        }

        // Publication d'un message sur un topic
        public async Task<bool> PublishAsync(string topic, string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .Build();

            MqttClientPublishResult result = await _client.PublishAsync(message);
            if (result.IsSuccess)
            {
                Console.WriteLine($"Message '{payload}' published on topic '{topic}'.");
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to publish message on topic '{topic}'.");
                return false;
            }
        }
    }
}
