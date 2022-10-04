using Newtonsoft.Json;

namespace GatewayWatchdog.Models
{
    public class Signal
    {
        [JsonProperty("4g")]
        public FourG? FourG { get; set; }
        [JsonProperty("5g")]
        public FiveG? FiveG { get; set; }
        public Generic? generic { get; set; }
    }
}
