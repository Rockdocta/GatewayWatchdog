using GatewayWatchdog.Models;
using GatewayWatchdog.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using TMobileAPI;

namespace GatewayWatchdog
{
    /// <summary>
    /// Interaction logic for Authentication.xaml
    /// </summary>
    public partial class Authentication : Window
    {
        public SessionInformation? Session { get; set; }
        public Authentication()
        {
            InitializeComponent();

            GatewayUrlText.Text = "http://192.168.12.1";
            AdminUsernameText.Text = "admin";          
        }


        public SessionInformation Authenticate(string url, string username, string password)
        {
            AuthenticationEngine authenticationEngine = new AuthenticationEngine(url);
            
            Session = authenticationEngine.Authenticate(username, password);           
            Credentials.Instance.IsInitialized = true;

            return Session;

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Credentials.Instance.Username = AdminUsernameText.Text;
                Credentials.Instance.Password = AdminPasswordText.Password;
                Credentials.Instance.GatewayUrl = GatewayUrlText.Text;

                Session = Authenticate(Credentials.Instance.GatewayUrl, Credentials.Instance.Username, Credentials.Instance.Password);               
                Close();
              
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error occurred while authenticating: " + exc.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
