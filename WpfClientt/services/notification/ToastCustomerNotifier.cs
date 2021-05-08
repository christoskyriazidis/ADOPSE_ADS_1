using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using WpfClientt.model;
using WpfClientt.services.notification;
using WpfClientt.viewModels;

namespace WpfClientt.services {
    class ToastCustomerNotifier : ICustomerNotifier {


        private Notifier notifier = new Notifier(cfg => {
            cfg.PositionProvider = new WindowPositionProvider(Application.Current.MainWindow,Corner.BottomRight,10,10);
            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(TimeSpan.FromSeconds(15), MaximumNotificationCount.FromCount(2));
            cfg.Dispatcher = Application.Current.Dispatcher;
            cfg.DisplayOptions.Width = 400;
        } );

        public void Error(string Error) {
            notifier.ShowError(Error);
        }

        public void Information(string info) {
            notifier.ShowInformation(info);
        }

        public void ChatRequestNotification(ChatRequest request, IChatService chatService) {
            notifier.ShowChatRequest(request, chatService);
        }

        public void Success(string success) {
            notifier.ShowSuccess(success);
        }

        public void Warning(string warning) {
            notifier.ShowWarning(warning);
        }

        
    }
}
