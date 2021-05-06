using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class NotificationsViewModel : IViewModel {
        private static NotificationsViewModel instance;

        private IChatService chatService;
        public ObservableCollection<NotificationViewModel> Notifications { get; set; } = new ObservableCollection<NotificationViewModel>();

        private NotificationsViewModel(IChatService chatService) {
            this.chatService = chatService;
            chatService.AddChatRequestListener(UpdateChatRequestsNotifications);
        }

        public static async Task<NotificationsViewModel> GetInstance(FactoryServices factory) {
            if(instance == null) {
                IChatService chatService = await factory.ChatServiceInstance();
                instance = new NotificationsViewModel(chatService);
            }
            await instance.UpdateChatRequestsNotifications();
            return instance;
        }

        private async Task UpdateChatRequestsNotifications() {
            Notifications.Clear();
            foreach (ChatRequest chatRequest in await chatService.ChatRequests()) {
                Notifications.Add(new ChatRequestNotificationViewModel(chatService, (_, request) => Notifications.Remove(request),chatRequest));
            }
        }


    }
}
