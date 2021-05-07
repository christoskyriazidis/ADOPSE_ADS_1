using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class RegisterViewModel : FormViewModel<RegisterForm> {

        private static RegisterViewModel instance;
        private ICustomerService customerService;
        public override RegisterForm Form { get; protected set; }


        private RegisterViewModel(ICustomerService customerService,ICustomerNotifier notifier) : base(notifier) {
            this.customerService = customerService;
            Form = new RegisterForm(Validate);
        }

        public static Task<RegisterViewModel> GetInstance(FactoryServices factory) {
            if (instance == null) {
                instance = new RegisterViewModel(factory.CustomerServiceInstance(),factory.CustomerNotifier());
            }
            return Task.FromResult(instance);
        }

        protected override Func<RegisterForm, Task> SubmitAction() => customerService.Register;

        protected override void ClearFormStrep() {
            Form = new RegisterForm(Validate);
        }
    }
}
