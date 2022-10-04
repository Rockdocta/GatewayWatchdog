using Newtonsoft.Json;

namespace GatewayWatchdog.Models
{
    public class Cell
    {
        [JsonProperty("4g")]
        public FourGCellInfo _4g { get; set; }

        [JsonProperty("5g")]
        public FiveGCellInfo _5g { get; set; }
        public Generic generic { get; set; }
        
    }


}
