using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfClientt.viewModels {
    public class GuestMenu : IMenu {
        public ICommand Categories { get; private set; }
        public ICommand Register { get; private set; }
        public ICommand Chats { get; private set; }
        public ICommand Login { get; private set; }
        public ICommand Back { get; private set; }

        public GuestMenu() {
            Categories = new AsyncCommand( async () => {
                await Mediator.Notify("CategoriesView");
            });
            Register = new AsyncCommand(async () => {
                await Mediator.Notify("RegisterView");  
            });
            Login = new AsyncCommand(async () => {
               await Mediator.Notify("LoginView");
            });
            Chats = new AsyncCommand(async () => await Mediator.Notify("ChatsView"));
            Back = new AsyncCommand(async () => await Mediator.Notify("BackView"));
        }

    }
}
