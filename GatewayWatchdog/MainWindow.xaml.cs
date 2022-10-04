using GatewayWatchdog.Models;
using GatewayWatchdog.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TMobileAPI;

namespace GatewayWatchdog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Timer that executes background workers
        readonly DispatcherTimer dispatcherTimer = new(DispatcherPriority.Background);

        // List of results that failed to log due to execution errors 
        private readonly List<string> cachedStrings = new();

        // Toggle for enabling Logging
        private bool _enableLogging = false;

        internal GatewayViewModel GatewayViewModel { get; set; }
        internal MainViewModel MainViewModel {get;set;}

        private Root? currentResult;
        private Root? previousResult;

        private DateTime nextQueryTime = DateTime.MinValue;
        private DateTime nextPingTime = DateTime.MinValue;

        private GatewayQueryWorker queryWorker;
        private PingWorker pingWorker;

        private SessionInformation? _sessionInformation;
        GatewayEngine _gatewayEngine = new GatewayEngine();
     
        public MainWindow()
        {
            GatewayViewModel = new GatewayViewModel();
            MainViewModel = new MainViewModel();

            InitializeComponent();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);

            queryWorker = new GatewayQueryWorker();
            queryWorker.WorkerUpdate += QueryWorker_WorkerUpdate;

            pingWorker = new PingWorker();
            pingWorker.PingCompleted += PingWorker_PingCompleted;

            dispatcherTimer.Start();               
        }

        private void PingWorker_PingCompleted(object? sender, PingResult e)
        {
            if (e.Result != null)
            {                
                PingResultText.Text = $"{DateTime.Now.ToLongTimeString()}: {e.Result.RoundtripTime}ms";
            }
            nextPingTime = DateTime.Now.AddMilliseconds(15000);
        }

        private void QueryWorker_WorkerUpdate(object? sender, WorkerResult e)
        {
            try
            {                   
                CurrentStatus.Text = e.Status;  
                if (e.HasError)
                {
                    GatewayViewModel.IsStale = true;
                    LogResult(e);
                }
                if (e.Data != null)
                {
                    GatewayViewModel.Initialize(e.Data);
                    
                    currentResult = e.Data;
                    ResultTime.Text = e.ResultTime.ToString();

                   GatewayViewModel.IsStale = false;

                    if (!currentResult.Equals(previousResult))
                    {
                        previousResult = currentResult;
                       
                        if (cachedStrings.Count > 0)
                        {
                            cachedStrings.ForEach(s => LogResult(s));
                            cachedStrings.Clear();
                        }

                        LogResult(e);
                    }
                    
                }
            }
            catch (IOException exc)
            {                
                cachedStrings.Add(e.ResultTime.ToString("M/dd/yyyy-hh:mm:ss.fff") + "," + e.Results + "\r\n");
            }
            finally
            {
                nextQueryTime = DateTime.Now.AddMilliseconds(2000);
            }
        }

        private void LogResult(WorkerResult result)
        {
            if (!_enableLogging)
                return;

            if (!File.Exists("GatewayLog.csv"))
                File.WriteAllText("GatewayLog.csv", "DATE," + GetColumnNames() + "\r\n");


            if (result.HasError)
                File.AppendAllText("GatewayLog.csv", result.ResultTime.ToString("M/dd/yyyy-hh:mm:ss.fff") + "," + result.Status + "\r\n");
            else
                File.AppendAllText("GatewayLog.csv", result.ResultTime.ToString("M/dd/yyyy-hh:mm:ss.fff") + "," + result.Results + "\r\n");
        }
        private void LogResult(string result)
        {
            if (!_enableLogging)
                return;
                    
            if (!File.Exists("GatewayLog.csv"))
                File.WriteAllText("GatewayLog.csv", "DATE," + GetColumnNames() + "\r\n");

             File.AppendAllText("GatewayLog.csv", result + "\r\n");
        }


        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            if (!queryWorker.IsBusy && nextQueryTime <= DateTime.Now)
                queryWorker.RunWorkerAsync();

            if (!pingWorker.IsBusy && nextPingTime <= DateTime.Now)
                pingWorker.RunWorkerAsync();

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
 
      

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = GatewayViewModel;
            UserAdminGrid.DataContext = MainViewModel;
            DeviceInfoControl.GatewayViewModel = GatewayViewModel;
        }

        private async void RestartGatewayBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_sessionInformation == null)
                    Authenticate();

                if (_sessionInformation != null)
                {

                    var confirmResponse = MessageBox.Show("This will reboot the gateway, your internet connection will be down for about 2 minutes. Do you wish to proceed?", "Confirm reboot", MessageBoxButton.YesNo);
                    if (confirmResponse != MessageBoxResult.Yes)
                    {
                        _gatewayEngine.Reboot(_sessionInformation);
                    }

                }

            }
            catch (Exception exc)
            {
                MessageBox.Show("Error occurred while attempting reboot: " + exc.Message);
            }

        }

        private void EnableLoggingBtn_Click(object sender, RoutedEventArgs e)
        {
            _enableLogging = !_enableLogging;
            EnableLoggingBtn.Content = _enableLogging ? "Disable Logging" : "Enable Logging";
        }

        private void ShowTelemetryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_sessionInformation == null)
                Authenticate();

            if (_sessionInformation != null)
            {
                var telemetryData = _gatewayEngine.GetAll(_sessionInformation);
                MessageBox.Show(JsonConvert.SerializeObject(telemetryData, Formatting.Indented));
            }
        }
        

        private void ShowCellBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_sessionInformation == null)
                Authenticate();

            if (_sessionInformation != null)
            {
                var telemetryData = _gatewayEngine.GetCells(_sessionInformation);
                MessageBox.Show(JsonConvert.SerializeObject(telemetryData, Formatting.Indented));
            }
        }

        private void ShowClientsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_sessionInformation == null)
                Authenticate();

            if (_sessionInformation != null)
            {
                var telemetryData = _gatewayEngine.GetDevices(_sessionInformation);
                MessageBox.Show(JsonConvert.SerializeObject(telemetryData, Formatting.Indented));
            }
        }

        private void Authenticate()
        {
            Authentication authenticate = new Authentication();
            authenticate.ShowDialog();

            _sessionInformation = authenticate.Session;
        }

        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_sessionInformation == null)
                Authenticate();

            if (_sessionInformation != null)
                SIMInformationControl.Initialize(_sessionInformation);           
        }
    }

}