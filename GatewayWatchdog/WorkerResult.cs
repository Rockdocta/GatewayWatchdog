using System;
using GatewayWatchdog.Models;

namespace GatewayWatchdog
{
    public class WorkerResult
    {

        public DateTime ResultTime { get; set; }
        public bool HasError { get; set; }
        public Root? Data { get; set; }
        public string? Results { get; set; }
        public string? Status { get; set; }
    }
}


