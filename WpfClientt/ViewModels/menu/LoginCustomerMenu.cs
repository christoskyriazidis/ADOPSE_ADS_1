using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class LoginCustomerMenu : BaseViewModel, IMenu {
        private static LoginCustomerMenu instance;

        private ICustomerNotifier notifier;
        public ICommand Categories { get; private set; }
        public ICommand Back { get; private set; }
        public ICommand Account { get; private set; }
        public ICommand CreateAd { get; private set; }
        public ICommand Notifications { get; private set; }
        public ICommand Chats { get; private set; }
        public ICommand Logout { get; private set; }
        public ICommand Subscriptions { get; private set; }

        private LoginCustomerMenu(IChatService chatService,ICustomerNotifier notifier) {
            this.notifier = notifier;
            Back = new AsyncCommand(async () => await Mediator.Notify("BackView"));
            Categories = new AsyncCommand(async () => await Mediator.Notify("CategoriesAdsViewModel"));
            Account = new AsyncCommand(async () => await Mediator.Notify("ProfileView"));
            CreateAd = new AsyncCommand(async () => await Mediator.Notify("CreateAdView"));
            Notifications = new AsyncCommand(async () => {
                await Mediator.Notify("NotificationsView");
            }
            );
            Chats = new AsyncCommand(async () => await Mediator.Notify("ChatsView"));
            Logout = new AsyncCommand(async () => await Mediator.Notify("Logout"));
            Subscriptions = new AsyncCommand(async () => await Mediator.Notify("CategoriesSubscriptionView"));
            chatService.AddChatRequestListener(ChatRequestListener);
            chatService.AddActiveChatListener(ActiveChatListener);
        }

        public static async Task<LoginCustomerMenu> GetInstance(FactoryServices factory) { 
            if(instance == null) {
                instance = new LoginCustomerMenu(await factory.ChatServiceInstance(),factory.CustomerNotifier());
            }

            return instance;
        }

        private Task ChatRequestListener(ChatRequest request) {
            notifier.Information($"New chat request from {request.Buyer.FirstName} {request.Buyer.LastName} for ad with title {request.Ad.Title}");
            return Task.CompletedTask;
        }

        private Task ActiveChatListener(Chat chat) {
            notifier.Success($"New chat with customer {chat.Customer.FirstName} {chat.Customer.LastName} has been established." +
                $"You can now chat!");
            return Task.CompletedTask;
        }
    }
}
