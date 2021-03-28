using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfClientt.viewModels {
    class DelegateCommand : ICommand {

        public Action<object> executeAction;

        public DelegateCommand(Action<object> executeAction) {
            this.executeAction = executeAction;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            this.executeAction.Invoke(parameter);
        }
    }
}
