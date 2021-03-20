using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using WpfApp;

namespace WpfClientt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OidcClient _oidcClient = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var options = new OidcClientOptions()
            {
                Authority = "https://localhost:44305/",
                ClientId = "wpf",
                Scope = "openid ApiOne credentials",
                RedirectUri = "http://localhost/sample-wpf-app",
                Browser = new WpfEmbeddedBrowser()
            };

            _oidcClient = new OidcClient(options);

            LoginResult result;
            try
            {
                result = await _oidcClient.LoginAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show( $"Unexpected Error: {ex.Message}");
                return;
            }

            if (result.IsError)
            {
                MessageBox.Show (result.Error == "UserCancel" ? "The sign-in window was closed before authorization was completed." : result.Error);

            }
            else
            {
                var name = result.User.Identity.Name;
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                MessageBox.Show($"Hello {name}");
                myLabel.Content = name;
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            try
            {
                var apiResult = await client.GetStringAsync("https://604b9553ee7cb900176a2603.mockapi.io/testt/users");
                var products = await client.GetStringAsync("https://604b9553ee7cb900176a2603.mockapi.io/testt/users");

                MessageBox.Show(products);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }


    }
}
