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
        public ICommand Login { get; private set; }
        public ICommand Back { get; private set; }

        public GuestMenu() {
            Categories = new AsyncCommand( async () => {
                await Mediator.Notify(MediatorToken.CategoriesAdsViewToken);
            });
            Register = new AsyncCommand(async () => {
                await Mediator.Notify(MediatorToken.RegisterViewToken);  
            });
            Login = new AsyncCommand(async () => {
               await Mediator.Notify(MediatorToken.LoginViewToken);
            });
            Back = new AsyncCommand(async () => await Mediator.Notify(MediatorToken.PreviousViewToken));
        }

    }
}
