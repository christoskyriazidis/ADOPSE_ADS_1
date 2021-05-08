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
        private IChatService chatService;
        public ICommand Categories { get; private set; }
        public ICommand Back { get; private set; }
        public ICommand Account { get; private set; }
        public ICommand CreateAd { get; private set; }
        public ICommand Notifications { get; private set; }
        public ICommand Chats { get; private set; }
        public ICommand Logout { get; private set; }
        public ICommand Subscriptions { get; private set; }

        private LoginCustomerMenu(IChatService chatSerivce,ICustomerNotifier notifier) {
            this.notifier = notifier;
            this.chatService = chatSerivce;
            Back = new AsyncCommand(async () => await Mediator.Notify(MediatorToken.PreviousViewToken));
            Categories = new AsyncCommand(async () => await Mediator.Notify(MediatorToken.CategoriesAdsViewToken));
            Account = new AsyncCommand(async () => await Mediator.Notify(MediatorToken.ProfileViewToken));
            CreateAd = new AsyncCommand(async () => await Mediator.Notify(MediatorToken.CreateAdViewToken));
            Notifications = new AsyncCommand(async () => { await Mediator.Notify(MediatorToken.NotificationsViewToken);});
            Chats = new AsyncCommand(async () => await Mediator.Notify(MediatorToken.ChatsViewToken));
            Logout = new AsyncCommand(async () => await Mediator.Notify(MediatorToken.LogoutViewToken));
            Subscriptions = new AsyncCommand(async () => await Mediator.Notify(MediatorToken.CategoriesSubscriptionsViewToken));
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
            notifier.ChatRequestNotification(request, chatService);
            return Task.CompletedTask;
        }

        private Task ActiveChatListener(Chat chat) {
            notifier.Success($"New chat with customer {chat.Customer.FirstName} {chat.Customer.LastName} has been established." +
                $"You can now chat!");
            return Task.CompletedTask;
        }
    }
}
