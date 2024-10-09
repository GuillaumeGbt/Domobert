using Common;

namespace BLL.Models.MQTT
{
    public class TypeAndTopic
    {
        public eDeviceType Type { get; set; }
        public string Topic { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is TypeAndTopic other)
            {
                return this.Type == other.Type && this.Topic == other.Topic;
            }
            return false;
        }

        // Surcharge de GetHashCode en fonction des propriétés
        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Topic);
        }
    }
}
