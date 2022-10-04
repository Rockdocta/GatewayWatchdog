using System.ComponentModel;

namespace GatewayWatchdog.Models
{
    public class Generic
    {
        [DisplayName("APN")]
        public string apn { get; set; }
        public bool hasIPv6 { get; set; }
        [DisplayName("REGISTRATION")]
        public string registration { get; set; }
        public bool roaming { get; set; }

    }
}
