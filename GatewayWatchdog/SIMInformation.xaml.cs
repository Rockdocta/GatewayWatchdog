using GatewayWatchdog.Models;
using GatewayWatchdog.ViewModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMobileAPI;
using static System.Collections.Specialized.BitVector32;

namespace GatewayWatchdog
{
    /// <summary>
    /// Interaction logic for SIMInformation.xaml
    /// </summary>
    public partial class SIMInformation : UserControl
    {
        internal SimInformationViewModel ViewModel { get; set; }
        public SIMInformation()
        {
            ViewModel = new();
            ViewModel.LoginVisibility = true;
            InitializeComponent();
        }

        public void Initialize(SessionInformation session)
        {
            GatewayEngine gatewayEngine = new GatewayEngine();
            
            var simInfo = gatewayEngine.GetSimInformation(session);
            ViewModel.Initialize(simInfo);            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SimGrid.DataContext = ViewModel;
        }

      
    }
}
