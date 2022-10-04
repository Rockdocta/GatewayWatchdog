using GatewayWatchdog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GatewayWatchdog.ViewModels
{
    internal class GatewayViewModel : ViewModelBase
    {
        public bool IsStale
        {
            get => isStale;
            set
            {
                isStale = value;
                RefreshAll();
            }
        }
        public void Initialize(Root root)
        {
            FourGBand = root.signal.FourG.band;
            FourGBars = root.signal.FourG.bars;
            FourGCID = root.signal.FourG.cid;
            FourGRSSI = root.signal.FourG.rssi;
            FourGRSRP = root.signal.FourG.rsrp;
            FourGRSRQ = root.signal.FourG.rsrq;
            FourGeBNID = root.signal.FourG.eNBID;
            FourGSINR = root.signal.FourG.sinr;

            FiveGBand = root.signal.FiveG.band;
            FiveGBars = root.signal.FiveG.bars;
            FiveGCID = root.signal.FiveG.cid;
            FiveGRSSI = root.signal.FiveG.rssi;
            FiveGRSRP = root.signal.FiveG.rsrp;
            FiveGRSRQ = root.signal.FiveG.rsrq;
            FiveGgBNID = root.signal.FiveG.gNBID;
            FiveGSINR = root.signal.FiveG.sinr;

            FriendlyName = root.device.friendlyName;
            HardwareVersion = root.device.hardwareVersion;
            IsEnabled = root.device.isEnabled;
            MACId = root.device.macId;
            Manufacturer = root.device.manufacturer;
            Model = root.device.model;
            Name = root.device.name;
            Role = root.device.role;
            SerialNumber = root.device.serial;
            SoftwareVersion = root.device.softwareVersion;
            Type = root.device.type;


        }

        private void RefreshAll()
        {
            PropertyInfo[] props = typeof(GatewayViewModel).GetProperties();
            foreach (var prop in props)
            {
                PropChanged(prop.Name);
            }
        }


        #region 4G Properties
        private string? _fourGBand;

        public string? FourGBand
        {
            get { return _fourGBand; }
            set { _fourGBand = value; PropChanged(nameof(FourGBand)); }
        }


        private double? _fourGBars;

        public double? FourGBars
        {
            get => _fourGBars;
            set
            {
                _fourGBars = value;
                PropChanged(nameof(FourGBars));
                PropChanged(nameof(FourGbarsStyle));
            }
        }

        private int? _fourGRSRP;

        public int? FourGRSRP
        {
            get => _fourGRSRP;
            set
            {
                _fourGRSRP = value;
                PropChanged(nameof(FourGRSRP));
                PropChanged(nameof(FourGrsrpStyle));
            }
        }

        private int? _fourGRSRQ;

        public int? FourGRSRQ
        {
            get => _fourGRSRQ;
            set
            {
                _fourGRSRQ = value;
                PropChanged(nameof(FourGRSRQ));
                PropChanged(nameof(FourGrsrqStyle));
            }
        }

        private int? _fourGRSSI;

        public int? FourGRSSI
        {
            get => _fourGRSSI;
            set
            {
                _fourGRSSI = value;
                PropChanged(nameof(FourGRSSI));
                PropChanged(nameof(FourGrssiStyle));
            }
        }

        private int? _fourGSINR;

        public int? FourGSINR
        {
            get => _fourGSINR;
            set
            {
                _fourGSINR = value;
                PropChanged(nameof(FourGSINR));
                PropChanged(nameof(FourGsinrStyle));
            }
        }

        private int? _fourGCID;

        public int? FourGCID
        {
            get => _fourGCID;
            set
            {
                _fourGCID = value;
                PropChanged(nameof(FourGCID));
            }
        }

        private int? _fourGeBNID;

        public int? FourGeBNID
        {
            get => _fourGeBNID;
            set
            {
                _fourGeBNID = value;
                PropChanged(nameof(FourGeBNID));
            }
        }

        public Style FourGbarsStyle => GetStyle(FourGBars, 2, 3, 4);
        public Style FourGrsrpStyle => GetStyle(FourGRSRP, -100, -90, -80);
        public Style FourGrsrqStyle => GetStyle(FourGRSRQ, -20, -15, -10);
        public Style FourGrssiStyle => GetStyle(FourGRSSI, -90, -80, -75);
        public Style FourGsinrStyle => GetStyle(FourGSINR, 0, 13, 20);


        #endregion

        #region 5G Properties
        private string? _fiveGBand;

        public string? FiveGBand
        {
            get { return _fiveGBand; }
            set { _fiveGBand = value; PropChanged(nameof(FiveGBand)); }
        }


        private double? _fiveGBars;

        public double? FiveGBars
        {
            get => _fiveGBars;
            set
            {
                _fiveGBars = value;
                PropChanged(nameof(FiveGBars));
                PropChanged(nameof(FiveGbarsStyle));
            }
        }

        private int? _fiveGRSRP;

        public int? FiveGRSRP
        {
            get => _fiveGRSRP;
            set
            {
                _fiveGRSRP = value;
                PropChanged(nameof(FiveGRSRP));
                PropChanged(nameof(FiveGrsrpStyle));
            }
        }

        private int? _fiveGRSRQ;

        public int? FiveGRSRQ
        {
            get => _fiveGRSRQ;
            set
            {
                _fiveGRSRQ = value;
                PropChanged(nameof(FiveGRSRQ));
                PropChanged(nameof(FiveGrsrqStyle));
            }
        }

        private int? _fiveGRSSI;

        public int? FiveGRSSI
        {
            get => _fiveGRSSI;
            set
            {
                _fiveGRSSI = value;
                PropChanged(nameof(FiveGRSSI));
                PropChanged(nameof(FiveGrssiStyle));
            }
        }

        private int? _fiveGSINR;

        public int? FiveGSINR
        {
            get => _fiveGSINR;
            set
            {
                _fiveGSINR = value;
                PropChanged(nameof(FiveGSINR));
                PropChanged(nameof(FiveGsinrStyle));
            }
        }

        private int? _fiveGgBNID;

        public int? FiveGgBNID
        {
            get => _fiveGgBNID;
            set
            {
                _fiveGgBNID = value;
                PropChanged(nameof(FiveGgBNID));
            }
        }

        private int? _fiveGCID;

        public int? FiveGCID
        {
            get => _fiveGCID;
            set
            {
                _fiveGCID = value;
                PropChanged(nameof(FiveGCID));
            }
        }


        public Style FiveGbarsStyle => GetStyle(FiveGBars, 2, 3, 4);
        public Style FiveGrsrpStyle => GetStyle(FiveGRSRP, -100, -90, -80);
        public Style FiveGrsrqStyle => GetStyle(FiveGRSRQ, -20, -15, -10);
        public Style FiveGrssiStyle => GetStyle(FiveGRSSI, -90, -80, -75);
        public Style FiveGsinrStyle => GetStyle(FiveGSINR, 0, 13, 20);



        #endregion

        #region Device Properties

        private string? _friendlyName;

        public string? FriendlyName
        {
            get => _friendlyName;
            set
            {
                _friendlyName = value;
                PropChanged(nameof(FriendlyName));
            }
        }
        private string? _hardwareVersion;

        public string? HardwareVersion
        {
            get => _hardwareVersion;
            set
            {
                _hardwareVersion = value;
                PropChanged(nameof(HardwareVersion));
            }
        }
        private bool? _isEnabled;

        public bool? IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                PropChanged(nameof(IsEnabled));
            }
        }

        private string? _isMeshSupported;

        public string? IsMeshSupported
        {
            get => _isMeshSupported;
            set
            {
                _isMeshSupported = value;
                PropChanged(nameof(IsMeshSupported));
            }
        }

        private string? _macId;

        public string? MACId
        {
            get => _macId;
            set
            {
                _macId = value;
                PropChanged(nameof(MACId));
            }
        }

        private string? _manufacturer;

        public string? Manufacturer
        {
            get => _manufacturer;
            set
            {
                _manufacturer = value;
                PropChanged(nameof(Manufacturer));
            }
        }

        private string? _manufacturerOUI;

        public string? ManufacturerOUI
        {
            get => _manufacturerOUI;
            set
            {
                _manufacturerOUI = value;
                PropChanged(nameof(ManufacturerOUI));
            }
        }

        private string? _model;

        public string? Model
        {
            get => _model;
            set
            {
                _model = value;
                PropChanged(nameof(Model));
            }
        }

        private string? _name;

        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                PropChanged(nameof(Name));
            }
        }

        private string? _role;

        public string? Role
        {
            get => _role;
            set
            {
                _role = value;
                PropChanged(nameof(Role));
            }
        }

        private string? _serialNumber;

        public string? SerialNumber
        {
            get => _serialNumber;
            set
            {
                _serialNumber = value;
                PropChanged(nameof(SerialNumber));
            }
        }

        private string? _softwareVersion;

        public string? SoftwareVersion
        {
            get => _softwareVersion;
            set
            {
                _softwareVersion = value;
                PropChanged(nameof(SoftwareVersion));
            }
        }

        private string? _type;

        public string? Type
        {
            get => _type;
            set
            {
                _type = value;
                PropChanged(nameof(Type));
            }
        }

        private string? _updateState;
        private bool isStale;

        public string? UpdateState
        {
            get => _updateState;
            set
            {
                _updateState = value;
                PropChanged(nameof(UpdateState));
            }
        }




        #endregion
        private Style GetStyle(double? value, double low, double mid, double high)
        {
            if (IsStale)
                return (Style)Application.Current.Resources["UndefinedTemplate"];

            if (value < low)
                return (Style)Application.Current.Resources["PoorTemplate"];
            else if (value < mid)
                return (Style)Application.Current.Resources["MidTemplate"];
            else if (value < high)
                return (Style)Application.Current.Resources["GoodTemplate"];
            else if (value >= high)
                return (Style)Application.Current.Resources["ExcellentTemplate"];
            else return (Style)Application.Current.Resources["UndefinedTemplate"];
        }
    }
}

