using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows;

namespace GatewayWatchdog.Models
{
    [DisplayName("5g")]
    public class FiveG : INotifyPropertyChanged
    {
        [DisplayName("5G BAND")]
        public string band { get { return bands.FirstOrDefault(); } }

        public List<string> bands { get; set; }


        [DisplayName("5G CID")]
        public int cid { get; set; }
        [DisplayName("5G gNBID")]
        public int gNBID { get; set; }


        private double _bars;
        private int _rsrp;
        private int _rsrq;
        private int _rssi;
        private int _sinr;

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };
        private void PropChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler(this, new PropertyChangedEventArgs(propertyName));
        }


        [DisplayName("5g BARS")]
        public double bars
        {
            get => _bars;
            set
            {
                _bars = value;
                PropChanged("barsStyle");
            }
        }




        [DisplayName("5g RSRP")]
        public int rsrp
        {
            get => _rsrp; set
            {
                _rsrp = value;
                PropChanged("rsrpStyle");
            }
        }



        [DisplayName("5g RSRQ")]
        public int rsrq
        {
            get => _rsrq;
            set
            {
                _rsrq = value;
                PropChanged("rsrqStyle");
            }
        }



        [DisplayName("5g RSSI")]
        public int rssi
        {
            get => _rssi;
            set
            {
                _rssi = value;
                PropChanged("rssiStyle");
            }
        }


        [DisplayName("5g SINR")]
        public int sinr
        {
            get => _sinr;
            set
            {
                _sinr = value;
                PropChanged("sinrStyle");
            }
        }



    }
}
