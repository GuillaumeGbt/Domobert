using System.ComponentModel.DataAnnotations;

namespace Blazor_Domobert.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public eDeviceType TypeCode { get; set; }
        public string Location { get; set; }
        public string TopicMQTT { get; set; }
    }
}
