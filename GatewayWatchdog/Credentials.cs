using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayWatchdog
{
    public class Credentials
    {
        private static Credentials? _instance;
        public static Credentials Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Credentials();

                return _instance;
            }
        }

        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? GatewayUrl { get; set; }

        public bool IsInitialized { get; set; }

    }
}
