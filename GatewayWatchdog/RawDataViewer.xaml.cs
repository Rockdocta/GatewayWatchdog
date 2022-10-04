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
using System.Windows.Shapes;

namespace GatewayWatchdog
{
    /// <summary>
    /// Interaction logic for RawDataViewer.xaml
    /// </summary>
    public partial class RawDataViewer : Window
    {
        public RawDataViewer()
        {
            InitializeComponent();
        }
        public RawDataViewer(string message)
        {
            InitializeComponent();
            ResultsText.Text = message;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
