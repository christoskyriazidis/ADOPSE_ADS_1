using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Core;
using WpfClientt.model;
using WpfClientt.viewModels;

namespace WpfClientt.services.notification {
    public static class ToastExtentionMethods {
        public static void ShowChatRequest(this Notifier notifier, ChatRequest request, IChatService chatService) {
            MessageOptions options = new MessageOptions();
            options.ShowCloseButton = true;
            notifier.Notify(() => new ChatRequestToastViewModel(options, chatService, request));
        }

        public static void ShowReviewAdNotification(this Notifier notifier,ReviewAdNotification notification) {
            MessageOptions options = new MessageOptions();
            options.ShowCloseButton = true;
            notifier.Notify(() => new AdReviewToastViewModel(options, notification));
        }
    }
}
