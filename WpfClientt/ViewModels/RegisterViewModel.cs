using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class RegisterViewModel : BaseViewModel,IViewModel {

        private static RegisterViewModel instance;

        private ICustomerService customerService;

        public string Username { get; set; }

        public string Password { get; set; }

        public string SecondPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        private RegisterViewModel(ICustomerService customerService) {
            this.customerService = customerService;
        }

        public async static Task<RegisterViewModel> GetInstance(FactoryServices factory) {
            if (instance == null) {
                instance = new RegisterViewModel(factory.CustomerServiceInstance());
            }
            return instance;
        }



    }
}
