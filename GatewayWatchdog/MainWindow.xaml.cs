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
using System.Windows.Input;
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

        private readonly GatewayQueryWorker queryWorker;
        private readonly PingWorker pingWorker;

        private SessionInformation? _sessionInformation;
        private SessionInformation Session
        {
            get
            {
                if (_sessionInformation != null)
                {
                    if (_sessionInformation.ExpireDateTime <= DateTime.Now)
                        _sessionInformation = null;                    
                }
                return _sessionInformation;
            }
            
        }
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
            queryWorker.GatewayUrl = "http://" + UrlText.Text;

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
            catch (IOException exc)  // Fires if the file is unavailable
            {                
                cachedStrings.Add(e.ResultTime.ToString("M/dd/yyyy-hh:mm:ss.fff") + "," + e.Results + "\r\n");
            }
            catch (Exception exc)
            {
                e.Results = exc.ToString();

            }
            finally
            {
                nextQueryTime = DateTime.Now.AddMilliseconds(2000);
            }
        }

        private void LogResult(WorkerResult result)
        {
            try
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
            catch (Exception exc)
            {
                CurrentStatus.Text = exc.Message;
            }
        }
        private void LogResult(string result)
        {
            try
            {
                if (!_enableLogging)
                    return;

                if (!File.Exists("GatewayLog.csv"))
                    File.WriteAllText("GatewayLog.csv", "DATE," + GetColumnNames() + "\r\n");

                File.AppendAllText("GatewayLog.csv", result + "\r\n");
            }
            catch (AggregateException exc)
            {
                CurrentStatus.Text = String.Join(",", exc.InnerExceptions.Select(e => e.Message));
            }
            catch (Exception exc)
            {
                CurrentStatus.Text = exc.Message;
            }
        }


        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (!queryWorker.IsBusy && nextQueryTime <= DateTime.Now)
                    queryWorker.RunWorkerAsync();

                if (!pingWorker.IsBusy && nextPingTime <= DateTime.Now)
                    pingWorker.RunWorkerAsync();
            }
            catch (AggregateException exc)
            {
                CurrentStatus.Text = String.Join(",", exc.InnerExceptions.Select(e => e.Message));
            }
            catch (Exception exc)
            {
                CurrentStatus.Text = exc.Message;
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
 
      

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DataContext = GatewayViewModel;
                UserAdminGrid.DataContext = MainViewModel;
                DeviceInfoControl.GatewayViewModel = GatewayViewModel;
            }
            catch (Exception exc)
            {
                CurrentStatus.Text = exc.Message;
            }
        }

        private void RestartGatewayBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Session == null)
                    Authenticate();

                if (Session != null)
                {

                    var confirmResponse = MessageBox.Show("This will reboot the gateway, your internet connection will be down for about 2 minutes. Do you wish to proceed?", "Confirm reboot", MessageBoxButton.YesNo);
                    if (confirmResponse == MessageBoxResult.Yes)
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
            try
            {
                _enableLogging = !_enableLogging;
                EnableLoggingBtn.Content = _enableLogging ? "Disable Logging" : "Enable Logging";
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error occurred on Enable Logging: " + exc.Message);
            }
        }

        private async void ShowTelemetryBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Session == null)
                    Authenticate();

                if (Session != null)
                {
                     
                     var telemetryData = await _gatewayEngine.GetAll(_sessionInformation);                    
                    var message = JsonConvert.SerializeObject(telemetryData, Formatting.Indented);
                    RawDataViewer rawDataViewer = new RawDataViewer(message);                    
                    rawDataViewer.ShowDialog();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error occurred: " + exc.Message);
            }
        }
        

        private async void ShowCellBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Session == null)
                    Authenticate();

                if (Session != null)
                {
                    var telemetryData = await _gatewayEngine.GetCells(_sessionInformation);
                    var message = JsonConvert.SerializeObject(telemetryData, Formatting.Indented);
                    RawDataViewer rawDataViewer = new RawDataViewer(message);
                    rawDataViewer.ShowDialog();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error occurred: " + exc.Message);
            }
        }

        private async void ShowClientsBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (Session == null)
                    Authenticate();

                if (Session != null)
                {
                    var telemetryData = await _gatewayEngine.GetDevices(_sessionInformation);
                    var message = JsonConvert.SerializeObject(telemetryData, Formatting.Indented);
                    RawDataViewer rawDataViewer = new RawDataViewer(message);
                    rawDataViewer.ShowDialog();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error occurred: " + exc.Message);
            }
        
        }

        private void Authenticate()
        {
            try
            {
                Authentication authenticate = new Authentication();
                if (Credentials.Instance.IsInitialized == false)
                {
                    authenticate.ShowDialog();
                }
                else
                    authenticate.Authenticate(Credentials.Instance.GatewayUrl, Credentials.Instance.Username, Credentials.Instance.Password);

                _sessionInformation = authenticate.Session;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error occurred: " + exc.Message);
            }
        }

        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Session == null)
                Authenticate();

            if (Session != null)
                SIMInformationControl.Initialize(_sessionInformation);           
        }

        private void SetUrlBtn_Click(object sender, RoutedEventArgs e)
        {
            queryWorker.GatewayUrl = "http://" + UrlText.Text;
            SetUrlBtn.IsEnabled = false;
        }

        private void UrlText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SetUrlBtn != null)
                SetUrlBtn.IsEnabled = true;
        }

        private void UrlText_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
                e.Key == Key.Decimal || e.Key == Key.OemPeriod || e.Key == Key.Back))
            {
                e.Handled = true;
            }
            
            if (UrlText.Text.Count(c => c == '.') >= 3 && (e.Key == Key.OemPeriod || e.Key == Key.Decimal))
            {
                e.Handled = true;
            }
        }

        private async void ShowNetworkConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Session == null)
                    Authenticate();

                if (Session != null)
                {
                    var telemetryData = await _gatewayEngine.GetAccessPointData(_sessionInformation);                    
                    RawDataViewer rawDataViewer = new RawDataViewer(telemetryData);
                    rawDataViewer.ShowDialog();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error occurred: " + exc.Message);
            }
        }
    }

}