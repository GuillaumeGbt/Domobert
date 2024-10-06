using Common;

namespace BLL.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public eDeviceType Type { get; set; }
        public string Location { get; set; }
        public string TopicMQTT { get; set; }
    }
}
