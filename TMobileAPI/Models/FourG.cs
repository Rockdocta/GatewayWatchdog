using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows;

namespace GatewayWatchdog.Models
{
    public class FourG : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        public FourG()
        {

        }

        private void PropChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        [DisplayName("4G BAND")]
        public string band { get { return bands.FirstOrDefault(); } }

        public List<string> bands { get; set; }

        [DisplayName("4G CID")]
        public int cid { get; set; }
        [DisplayName("4G eNBID")]
        public int eNBID { get; set; }

        private double _bars;
        private int _rsrp;
        private int _rsrq;
        private int _rssi;
        private int _sinr;

        [DisplayName("4G BARS")]
        public double bars
        {
            get => _bars;
            set
            {
                _bars = value;
                PropChanged("barsStyle");
            }
        }


        [DisplayName("4G RSRP")]
        public int rsrp
        {
            get => _rsrp; set
            {
                _rsrp = value;
                PropChanged("rsrpStyle");
            }
        }




        [DisplayName("4G RSRQ")]
        public int rsrq
        {
            get => _rsrq;
            set
            {
                _rsrq = value;
                PropChanged("rsrqStyle");
            }
        }



        [DisplayName("4G RSSI")]
        public int rssi
        {
            get => _rssi;
            set
            {
                _rssi = value;
                PropChanged("rssiStyle");
            }
        }


        [DisplayName("4G SINR")]
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
