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
        private INotificationService notificationService;
        public ObservableCollection<NotificationViewModel> Notifications { get; set; } = new ObservableCollection<NotificationViewModel>();

        private NotificationsViewModel(IChatService chatService, INotificationService notificationService) {
            Mediator.Subscribe(MediatorToken.ChatRequestManagedInToastToken, RemoveChatRequest);
            Mediator.Subscribe(MediatorToken.ChatRequestManagedInNotificationsToken, RemoveChatRequest);
            Mediator.Subscribe(MediatorToken.ReviewSentToken, RemoveReviewNotification);
            this.chatService = chatService;
            this.notificationService = notificationService;
            chatService.AddChatRequestListener(UpdateChatRequestsNotifications);
            notificationService.AddReviewNotificationListener(UpdateReviewAdNotifications);
        }

        public static async Task<NotificationsViewModel> GetInstance(FactoryServices factory) {
            if(instance == null) {
                IChatService chatService = await factory.ChatServiceInstance();
                INotificationService notificationService = await factory.NotificationService();
                instance = new NotificationsViewModel(chatService, notificationService);
            }
            instance.Notifications.Clear();
            await instance.LoadChatRequests();
            await instance.LoadAdReviewNotifications();
            return instance;
        }

        private Task RemoveChatRequest(object param) {
            ChatRequest request = (ChatRequest)param;
            NotificationViewModel found = null;
            foreach(NotificationViewModel notificationViewModel in Notifications) {
                if(notificationViewModel is ChatRequestNotificationViewModel rvm && rvm.ChatRequest.Id.Equals(request.Id)) {
                    found = notificationViewModel;
                    break;
                }
            }
            if(found != null) {
                Notifications.Remove(found);
            }
            return Task.CompletedTask;
        }

        private Task RemoveReviewNotification(object param) {
            Ad ad = (Ad)param;
            NotificationViewModel found = null;

            foreach (NotificationViewModel notificationViewModel in Notifications) {
                if (notificationViewModel is AdReviewNotificationViewModel arn && arn.ReviewNotification.Ad.Id.Equals(ad.Id)) {
                    found = notificationViewModel;
                    break;
                }
            }
            if (found != null) {
                Notifications.Remove(found);
            }
            return Task.CompletedTask;
        }

        private Task UpdateChatRequestsNotifications(ChatRequest chatRequest) {
            Notifications.Insert(0,new ChatRequestNotificationViewModel(chatService,chatRequest));
            return Task.CompletedTask;
        }

        private Task UpdateReviewAdNotifications(ReviewAdNotification reviewAdNotification) {
            Notifications.Insert(0,new AdReviewNotificationViewModel(reviewAdNotification));
            return Task.CompletedTask;
        }

        private async Task LoadChatRequests() {
            foreach (ChatRequest chatRequest in await chatService.ChatRequests()) {
                Notifications.Insert(0,new ChatRequestNotificationViewModel(chatService, chatRequest));
            }
        }

        private async Task LoadAdReviewNotifications() { 
            foreach(ReviewAdNotification notification in await notificationService.ReviewAdNotifications()) {
                Notifications.Insert(0,  new AdReviewNotificationViewModel(notification));
            }
        }


    }
}
