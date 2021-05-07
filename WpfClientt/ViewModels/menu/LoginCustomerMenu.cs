using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class LoginCustomerMenu : BaseViewModel, IMenu {
        private static LoginCustomerMenu instance;

        public ICommand Categories { get; private set; }
        public ICommand Back { get; private set; }
        public ICommand Account { get; private set; }
        public ICommand CreateAd { get; private set; }
        public ICommand Notifications { get; private set; }
        public ICommand Chats { get; private set; }
        public ICommand Logout { get; private set; }

        public ICommand Subscriptions { get; private set; }

        public string Message { get; set; } = string.Empty;

        private LoginCustomerMenu(IChatService chatService) {
            Back = new AsyncCommand(async () => await Mediator.Notify("BackView"));
            Categories = new AsyncCommand(async () => await Mediator.Notify("CategoriesView"));
            Account = new AsyncCommand(async () => await Mediator.Notify("ProfileView"));
            CreateAd = new AsyncCommand(async () => await Mediator.Notify("CreateAdView"));
            Notifications = new AsyncCommand(async () => {
                await Mediator.Notify("NotificationsView");
                Message = string.Empty;
            }
            );
            Chats = new AsyncCommand(async () => await Mediator.Notify("ChatsView"));
            Logout = new AsyncCommand(async () => await Mediator.Notify("Logout"));
            Subscriptions = new AsyncCommand(async () => await Mediator.Notify("SubscriptionView"));
            chatService.AddChatRequestListener(ChatRequestListener);
        }

        public static async Task<LoginCustomerMenu> GetInstance(FactoryServices factory) { 
            if(instance == null) {
                instance = new LoginCustomerMenu(await factory.ChatServiceInstance());
            }

            return instance;
        }

        private Task ChatRequestListener() {
            Message = "(new)";
            OnPropertyChanged(nameof(Message));
            return Task.CompletedTask;
        }
    }
}
