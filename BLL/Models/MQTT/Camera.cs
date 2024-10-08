using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.MQTT
{
    internal class Camera
    {
        public string image { get; set; }
        public DateTime timestamp { get; set; }
    }
}
