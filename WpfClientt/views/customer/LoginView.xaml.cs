using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
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
        private bool firedOnce = false;
        public LoginView() {
            InitializeComponent();
            DataContextChanged += DataContextListener;
        }

        private void DataContextListener(object sender, DependencyPropertyChangedEventArgs e) {
            if (!firedOnce) {
                firedOnce = true;
                browser.Source = new Uri((string)TypeDescriptor
                                        .GetProperties(DataContext)["LoginUrl"]
                                        .GetValue(DataContext));
            }
        }

        private void browser_Navigated(object sender, NavigationEventArgs e) {
            string redirectUrl = browser.Source.AbsoluteUri;
            NameValueCollection queryValues = HttpUtility.ParseQueryString(redirectUrl.Substring(redirectUrl.IndexOf("?") + 1));
            if (queryValues.Get("code") != null) {
               ICommand command = (ICommand)TypeDescriptor.GetProperties(DataContext)["ExchangeTokenCommand"]
                              .GetValue(DataContext);
                command.Execute(e.Uri.AbsoluteUri);
            }else if(queryValues.Get("error") != null) {

            }
        }
    }
}
