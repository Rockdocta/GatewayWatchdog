using GatewayWatchdog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayWatchdog.ViewModels
{
    internal class SimInformationViewModel : ViewModelBase
    {
        private bool _loginVisibility;

        public bool LoginVisibility
        {
            get => _loginVisibility;
            set
            {
                _loginVisibility = value;
                PropChanged(nameof(LoginVisibility));
            }
        }

        private string? _iccId;

        public string? IccId
        {
            get => _iccId;
            set
            {
                _iccId = value;
                PropChanged(nameof(IccId));
            }
        }

        private string _imei;

        public string IMEI
        {
            get => _imei;
            set
            {
                _imei = value;
                PropChanged(nameof(IMEI));
            }
        }

        private string _imsi;

        public string IMSI
        {
            get => _imsi;
            set
            {
                _imsi = value;
                PropChanged(nameof(IMSI));
            }
        }

        private string? _msisdn;

        public string? MSISDN
        {
            get => _msisdn;
            set
            {
                _msisdn = value;
                PropChanged(nameof(MSISDN));
            }
        }

        internal void Initialize(SimRoot? simData)
        {
            IccId = simData.sim.iccId;
            MSISDN = simData.sim.msisdn;
            IMEI = simData.sim.imei;
            IMSI = simData.sim.imsi;
        }
    }
}
