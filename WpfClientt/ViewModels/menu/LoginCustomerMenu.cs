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

        public string NumberOfNotifications { get; set; } = string.Empty;

        private LoginCustomerMenu(IChatService chatService) {
            Back = new DelegateCommand(_ => Mediator.Notify("BackView"));
            Categories = new DelegateCommand(_ => Mediator.Notify("CategoriesView"));
            Account = new DelegateCommand(_ => Mediator.Notify("ProfileView"));
            CreateAd = new DelegateCommand(_ => Mediator.Notify("CreateAdView"));
            Notifications = new DelegateCommand(_ => Mediator.Notify("NotificationsView"));
            Chats = new DelegateCommand(_ => Mediator.Notify("ChatsView"));
            Logout = new DelegateCommand(_ => Mediator.Notify("Logout"));
            chatService.AddChatRequestListener(ChatRequestListener);
        }

        public static async Task<LoginCustomerMenu> GetInstance(FactoryServices factory) { 
            if(instance == null) {
                instance = new LoginCustomerMenu(await factory.ChatServiceInstance());
            }

            return instance;
        }

        private async Task ChatRequestListener() {
            NumberOfNotifications = NumberOfNotifications.Equals(string.Empty) ? "(1)" : NextValue();
            OnPropertyChanged(nameof(NumberOfNotifications));
        }

        private string NextValue() {
            int indexOfSecondParenthesis = NumberOfNotifications.IndexOf(")");
            return $"({int.Parse(NumberOfNotifications.Substring(1, indexOfSecondParenthesis - 1))})";
        }
    }
}
