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
            Mediator.Subscribe("ChatRequestDelete", RemoveChatRequest);
            this.chatService = chatService;
            chatService.AddChatRequestListener(UpdateChatRequestsNotifications);
        }

        public static async Task<NotificationsViewModel> GetInstance(FactoryServices factory) {
            if(instance == null) {
                IChatService chatService = await factory.ChatServiceInstance();
                instance = new NotificationsViewModel(chatService);
            }
            await instance.LoadChatRequests();
            return instance;
        }

        private Task RemoveChatRequest(object param) {
            ChatRequest request = (ChatRequest)param;
            foreach(NotificationViewModel notificationViewModel in Notifications) {
                if(notificationViewModel is ChatRequestNotificationViewModel rvm && rvm.ChatRequest.Id.Equals(request.Id)) {
                    Notifications.Remove(notificationViewModel);
                    break;
                }
            }
            return Task.CompletedTask;
        }

        private Task UpdateChatRequestsNotifications(ChatRequest chatRequest) {
            Notifications.Insert(0,new ChatRequestNotificationViewModel(chatService, (_, request) => Notifications.Remove(request), chatRequest));
            return Task.CompletedTask;
        }

        private async Task LoadChatRequests() {
            foreach (ChatRequest chatRequest in await chatService.ChatRequests()) {
                Notifications.Add(new ChatRequestNotificationViewModel(chatService, (_, request) => Notifications.Remove(request), chatRequest));
            }
        }


    }
}
