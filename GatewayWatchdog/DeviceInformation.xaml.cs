using GatewayWatchdog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace GatewayWatchdog
{
    /// <summary>
    /// Interaction logic for DeviceInformation.xaml
    /// </summary>
    public partial class DeviceInformation : UserControl
    {
        internal GatewayViewModel GatewayViewModel { get; set; }

        public DeviceInformation()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DeviceGrid.DataContext = GatewayViewModel;
        }
    }
}
