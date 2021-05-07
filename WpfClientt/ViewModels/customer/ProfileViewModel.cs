using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class ProfileViewModel : IViewModel {
        private static ProfileViewModel profileViewModel;

        private ICustomerService customerService;

        private ProfileViewModel(ICustomerService customerService) {
            this.customerService = customerService;
        }

        public static Task<ProfileViewModel> GetInstance(FactoryServices factoryServices) {
            if(profileViewModel == null) {
                profileViewModel = new ProfileViewModel(factoryServices.CustomerServiceInstance());
            }

            return Task.FromResult(profileViewModel);
        }

    }
}
