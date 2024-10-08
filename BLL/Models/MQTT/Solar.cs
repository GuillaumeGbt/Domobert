using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.MQTT
{
    public class Solar
    {
        public DateTime time { get; set; }
        public double production { get; set; }
    }
}
