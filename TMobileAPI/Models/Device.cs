using System.ComponentModel;

namespace GatewayWatchdog.Models
{
    public class Device
    {
        [DisplayName(nameof(friendlyName))]
        public string friendlyName { get; set; }

        [DisplayName(nameof(hardwareVersion))]
        public string hardwareVersion { get; set; }

        [DisplayName(nameof(isEnabled))] public bool isEnabled { get; set; }
        [DisplayName(nameof(isMeshSupported))] public bool isMeshSupported { get; set; }
        [DisplayName(nameof(macId))] public string macId { get; set; }
        [DisplayName(nameof(manufacturer))] public string manufacturer { get; set; }
        [DisplayName(nameof(manufacturerOUI))] public string manufacturerOUI { get; set; }
        [DisplayName(nameof(model))] public string model { get; set; }
        [DisplayName(nameof(name))] public string name { get; set; }
        [DisplayName(nameof(role))] public string role { get; set; }
        [DisplayName(nameof(serial))] public string serial { get; set; }
        [DisplayName(nameof(softwareVersion))] public string softwareVersion { get; set; }
        [DisplayName(nameof(type))] public string type { get; set; }
        [DisplayName(nameof(updateState))] public string updateState { get; set; }
    }
}
