using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfClientt.viewModels.menu {
    public class GuestMenu : IMenu {
        public ICommand Categories { get; private set; }
        public ICommand Register { get; private set; }
        public ICommand Login { get; private set; }
        public ICommand Back { get; private set; }

        public GuestMenu() {
            Categories = new DelegateCommand(obj => {
                Mediator.Notify("CategoriesView");
            });
            Register = new DelegateCommand(obj => {
                Mediator.Notify("RegisterView");  
            });
            Login = new DelegateCommand(obj => {
                Mediator.Notify("LoginView");
            });
            Back = new DelegateCommand(obj => Mediator.Notify("BackView"));
        }

    }
}
