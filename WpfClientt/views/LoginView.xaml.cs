using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WpfClientt.views {
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl {
        public LoginView() {
            InitializeComponent();
        }

        private void WebBrowser_LoadCompleted(object sender, NavigationEventArgs e) {
            if (browser.Source.Equals("https://www.google.com/")) {
                string LoginUrl = (string)TypeDescriptor
                                        .GetProperties(DataContext)["LoginUrl"]
                                        .GetValue(DataContext);
                browser.Navigate(LoginUrl);
            }
        }

        private void browser_Navigated(object sender, NavigationEventArgs e) {
            if (e.Uri.AbsoluteUri.Contains("code") && !e.Uri.AbsoluteUri.Contains("challenge")) {
               ICommand command = (ICommand)TypeDescriptor.GetProperties(DataContext)["ExchangeTokenCommand"]
                              .GetValue(DataContext);
                command.Execute(e.Uri.AbsoluteUri);
            }
        }
    }
}
