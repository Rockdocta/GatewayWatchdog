using System.Collections.Generic;

namespace GatewayWatchdog.Models
{
    public class FourGCellInfo
    {
        public string bandwidth { get; set; }
        public int cqi { get; set; }
        public string earfcn { get; set; }
        public string ecgi { get; set; }
        public string mcc { get; set; }
        public string mnc { get; set; }
        public string pci { get; set; }
        public string plmn { get; set; }
        public Sector sector { get; set; }
        public bool status { get; set; }
        public List<string> supportedBands { get; set; }
        public string tac { get; set; }
    }


}
