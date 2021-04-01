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
        private RegisterViewModel() {

        }

        public async static Task<RegisterViewModel> GetInstance(FactoryServices factory) {
            if (instance == null) {
                instance = new RegisterViewModel();
            }
            return instance;
        }



    }
}
