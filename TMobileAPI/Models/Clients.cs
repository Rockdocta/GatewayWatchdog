using Newtonsoft.Json;
using System.Collections.Generic;

namespace GatewayWatchdog.Models
{
    public class Clients
    {
        [JsonProperty("2.4ghz")]
        public List<object> _24ghz { get; set; }

        [JsonProperty("5.0ghz")]
        public List<object> _50ghz { get; set; }
        public List<Ethernet> ethernet { get; set; }
    }


}
