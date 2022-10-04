using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GatewayWatchdog
{
    internal class PingWorker : BackgroundWorker
    {
        public event EventHandler<PingResult> PingCompleted;
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            IPAddress pingAddress = IPAddress.Parse("8.8.8.8");
            try
            {
                var pingResult = PingTest(pingAddress);
                e.Result = new PingResult
                {
                    PingUri = pingAddress,
                    Result = pingResult
                };
                
            }
            catch (Exception exc)
            {
                e.Result = new PingResult
                {
                    PingUri = pingAddress,
                    Error = exc.Message
                };
            }
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            var pingResult = e.Result as PingResult;
            if (pingResult != null)
                PingCompleted(this, pingResult);
        }

        private PingReply PingTest(IPAddress pingUri)
        {
            Ping ping = new Ping();
            PingReply reply = ping.Send(pingUri);

            return reply;
        }       
    }
}
