using Newtonsoft.Json;

namespace GatewayWatchdog.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Auth
    {
        [JsonProperty("expiration")]
        public int Expiration { get; set; }
        
        [JsonProperty("refreshCountLeft")]
        internal int RefreshCountLeft { get; set; }

        [JsonProperty("refreshCountMax")]
        internal int RefreshCountMax { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }

}