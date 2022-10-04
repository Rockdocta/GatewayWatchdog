using System.ComponentModel;

namespace GatewayWatchdog.Models
{
    public class Time
    {
        [DisplayName(nameof(daylightSavings))] public DaylightSavings daylightSavings { get; set; }
        [DisplayName(nameof(localTime))] public int localTime { get; set; }
        [DisplayName(nameof(localTimeZone))] public string localTimeZone { get; set; }
        [DisplayName(nameof(upTime))] public int upTime { get; set; }
    }
}
