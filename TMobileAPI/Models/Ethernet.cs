using System.Collections.Generic;

namespace GatewayWatchdog.Models
{
    public class Ethernet
    {
        public bool connected { get; set; }
        public string ipv4 { get; set; }
        public List<string> ipv6 { get; set; }
        public string mac { get; set; }
        public string name { get; set; }
    }


}
