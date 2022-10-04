using System.Collections.Generic;

namespace GatewayWatchdog.Models
{
    public class TelemetryData
    {
        public Cell cell { get; set; }
        public Clients clients { get; set; }
        public List<Neighbor> neighbors { get; set; }
        public Sim sim { get; set; }
    }


}
