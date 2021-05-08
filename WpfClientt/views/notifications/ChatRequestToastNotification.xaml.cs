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
using ToastNotifications.Core;
using WpfClientt.viewModels;

namespace WpfClientt.views.notifications {
    /// <summary>
    /// Interaction logic for ChatRequestToastNotification.xaml
    /// </summary>
    public partial class ChatRequestToastNotification : NotificationDisplayPart {
        public ChatRequestToastNotification(ChatRequestToastViewModel viewmodel) {
            InitializeComponent();
            Bind(viewmodel);
        }

        private void CloseNotification(object sender, RoutedEventArgs e) {
            OnClose();
        }
    }
}
