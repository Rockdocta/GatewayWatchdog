using GatewayWatchdog.Models;
using GatewayWatchdog.ViewModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TMobileAPI;

namespace GatewayWatchdog
{
    public class GatewayQueryWorker : BackgroundWorker
    {
        public event EventHandler<WorkerResult> WorkerUpdate;

        private GatewayEngine gatewayEngine;

        public GatewayQueryWorker()
        {

            DoWork += BackgroundWorker_DoWork;
            ProgressChanged += BackgroundWorker_ProgressChanged;
            RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            WorkerReportsProgress = true;

            gatewayEngine = new GatewayEngine();

        }

        private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            WorkerResult? result = e.Result as WorkerResult;
            if (WorkerUpdate != null)
                WorkerUpdate(this, result);
        }

        private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if (WorkerUpdate != null)
                WorkerUpdate(this, new WorkerResult { Status = e.UserState.ToString() });
        }

        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            var currentTimeStamp = DateTime.Now;
            string toWrite = "";
            string data = "";
            string telemetry = "";
            Root? root = null;
            try
            {
                var bw = sender as BackgroundWorker;
                bw.ReportProgress(1, "Sending request");
                root = gatewayEngine.GetGatewayInformation("http://192.168.12.1").Result;               
                bw.ReportProgress(2, "Parsing results");

                PropertyInfo[] _4gProperties = root.signal.FourG.GetType().GetProperties();
                PropertyInfo[] _5gProperties = root.signal.FiveG.GetType().GetProperties();
                PropertyInfo[] deviceProperties = root.device.GetType().GetProperties();
                PropertyInfo[] timeProperties = root.time.GetType().GetProperties();
                PropertyInfo[] genericProperties = root.signal.generic.GetType().GetProperties();

                toWrite = string.Join(",", timeProperties
                                    .Where(p => p.GetCustomAttributes<DisplayNameAttribute>()
                                                 .Select(att => att.DisplayName).FirstOrDefault() != null)
                                    .Select(p => p.GetValue(root.time))) + "," +
                           string.Join(",", _4gProperties
                                    .Where(p => p.GetCustomAttributes<DisplayNameAttribute>()
                                                 .Select(att => att.DisplayName).FirstOrDefault() != null)
                                    .Select(p => p.GetValue(root.signal.FourG))) + "," +
                           string.Join(",", _5gProperties
                                    .Where(p => p.GetCustomAttributes<DisplayNameAttribute>()
                                                 .Select(att => att.DisplayName).FirstOrDefault() != null)
                                    .Select(p => p.GetValue(root.signal.FiveG))) + "," +
                           string.Join(",", genericProperties
                                    .Where(p => p.GetCustomAttributes<DisplayNameAttribute>()
                                                 .Select(att => att.DisplayName).FirstOrDefault() != null)
                                    .Select(p => p.GetValue(root.signal.generic))) + "," +
                           string.Join(",", deviceProperties
                                    .Where(p => p.GetCustomAttributes<DisplayNameAttribute>()
                                                 .Select(att => att.DisplayName).FirstOrDefault() != null)
                                    .Select(p => p.GetValue(root.device)));



                e.Result = new WorkerResult
                {
                    ResultTime = currentTimeStamp,
                    HasError = false,
                    Data = root,
                    Results = toWrite,
                    Status = "Results received"

                };
            }



            catch (AggregateException exc)
            {
                e.Result = new WorkerResult
                {
                    ResultTime = currentTimeStamp,
                    HasError = true,
                    Status = "Error: " + exc.Message + ": " + String.Join(",", exc.InnerExceptions.Select(ex => ex.Message)),
                    Data = root
                };
            }
            catch (Exception exc)
            {
                e.Result = new WorkerResult
                {
                    ResultTime = currentTimeStamp,
                    HasError = true,
                    Status = "Error: " + exc.GetType().Name + ": " + exc.Message,
                    Data = root
                };
            }

        }


        public string GetColumnNames()
        {
            StringBuilder sb = new StringBuilder();

            var genericProps = typeof(Generic).GetProperties();
            var _4gProps = typeof(FourG).GetProperties();
            var _5gProps = typeof(FiveG).GetProperties();
            var deviceProps = typeof(Device).GetProperties();
            var timeProps = typeof(Time).GetProperties();

            return string.Join(",", timeProps.Where(p => p.GetCustomAttributes<DisplayNameAttribute>().Select(att => att.DisplayName).FirstOrDefault() != null).Select(p => p.GetCustomAttribute<DisplayNameAttribute>().DisplayName)) + "," +
                   string.Join(",", _4gProps.Where(p => p.GetCustomAttributes<DisplayNameAttribute>().Select(att => att.DisplayName).FirstOrDefault() != null).Select(p => p.GetCustomAttribute<DisplayNameAttribute>().DisplayName)) + "," +
                   string.Join(",", _5gProps.Where(p => p.GetCustomAttributes<DisplayNameAttribute>().Select(att => att.DisplayName).FirstOrDefault() != null).Select(p => p.GetCustomAttribute<DisplayNameAttribute>().DisplayName)) + "," +
                   string.Join(",", genericProps.Where(p => p.GetCustomAttributes<DisplayNameAttribute>().Select(att => att.DisplayName).FirstOrDefault() != null).Select(p => p.GetCustomAttribute<DisplayNameAttribute>().DisplayName)) + "," +
                   string.Join(",", deviceProps.Where(p => p.GetCustomAttributes<DisplayNameAttribute>().Select(att => att.DisplayName).FirstOrDefault() != null).Select(p => p.GetCustomAttribute<DisplayNameAttribute>().DisplayName));
        }


       
    }
}


