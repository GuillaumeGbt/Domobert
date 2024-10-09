using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor_Domobert.Models.SignalR
{
    public class SolarData
    {
        public DateTime Timestamp { get; set; }
        public double production { get; set; }
    }
}
