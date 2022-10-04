using System;

namespace GatewayWatchdog.Models
{
    public class Root : IEquatable<Root>
    {
        public Signal signal { get; set; }
        public Device device { get; set; }
        public Time time { get; set; }

        public override bool Equals(object? obj) => Equals(obj as Root);
        public bool Equals(Root? other)
        {
            if (other == null) return false;
            bool equal = 
                other.signal.FourG.band == signal.FourG.band &&
                other.signal.FourG.bars == signal.FourG.bars &&
                other.signal.FourG.eNBID == signal.FourG.eNBID &&
                other.signal.FourG.cid == signal.FourG.cid &&
                other.signal.FourG.rssi == signal.FourG.rssi &&
                other.signal.FourG.rsrp == signal.FourG.rsrp &&
                other.signal.FourG.rsrq == signal.FourG.rsrq &&
                other.signal.FourG.sinr == signal.FourG.sinr &&
                other.signal.FiveG.band == signal.FiveG.band &&
                other.signal.FiveG.bars == signal.FiveG.bars &&
                other.signal.FiveG.cid == signal.FiveG.cid &&
                other.signal.FiveG.rssi == signal.FiveG.rssi &&
                other.signal.FiveG.rsrp == signal.FiveG.rsrp &&
                other.signal.FiveG.rsrq == signal.FiveG.rsrq &&
                other.signal.FiveG.sinr == signal.FiveG.sinr;
            return equal;
        }
    }
}
