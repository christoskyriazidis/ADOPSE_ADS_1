using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfClientt.viewModels {
    public class LoginCustomerMenu : IMenu {

        public ICommand Categories { get; private set; }
        public ICommand Back { get; private set; }
        public ICommand Account { get; private set; }
        public ICommand CreateAd { get; private set; }
        public ICommand Notifications { get; private set; }
        public ICommand Chats { get; private set; }
        public ICommand Logout { get; private set; }

        public LoginCustomerMenu() {
            Back = new DelegateCommand(_ => Mediator.Notify("BackView"));
            Categories = new DelegateCommand(_ => Mediator.Notify("CategoriesView"));
            Account = new DelegateCommand(_ => Mediator.Notify("ProfileView"));
            CreateAd = new DelegateCommand(_ => Mediator.Notify("CreateAdView"));
            Notifications = new DelegateCommand(_ => Mediator.Notify("NotificationsView"));
            Chats = new DelegateCommand(_ => Mediator.Notify("ChatsView"));
            Logout = new DelegateCommand(_ => Mediator.Notify("Logout"));
        }

    }
}
