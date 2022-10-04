using System.Net;
using System.Net.NetworkInformation;

namespace GatewayWatchdog
{
    public class PingResult
    {
        public IPAddress PingUri { get; set; }
        public PingReply Result { get; set; }
        public string Error { get; set; }
    }
}


