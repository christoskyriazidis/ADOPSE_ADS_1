using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfClientt.viewModels.menu {
    public class GuestMenu : IMenu {
        public ICommand CategoriesView { get; private set; }
        public ICommand RegisterView { get; private set; }
        public ICommand LoginView { get; private set; }
        public ICommand BackView { get; private set; }

        public GuestMenu() {
            CategoriesView = new DelegateCommand(obj => {
                Mediator.Notify("CategoriesView");
            });
            RegisterView = new DelegateCommand(obj => {
                Mediator.Notify("RegisterView");  
            });
            LoginView = new DelegateCommand(obj => {
                Mediator.Notify("LoginView");
            });
            LoginView = new DelegateCommand(obj => {
                Mediator.Notify("LoginView");
            });
            BackView = new DelegateCommand(obj => Mediator.Notify("BackView"));
        }

    }
}
