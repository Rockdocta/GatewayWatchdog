using System.Collections.Generic;

namespace GatewayWatchdog.Models
{
    public class Sector
    {
        public List<string> bands { get; set; }
        public double bars { get; set; }
        public int cid { get; set; }
        public int eNBID { get; set; }
        public int rsrp { get; set; }
        public int rsrq { get; set; }
        public int rssi { get; set; }
        public int sinr { get; set; }
        public int gNBID { get; set; }
    }


}
