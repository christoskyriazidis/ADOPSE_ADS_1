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

        public async static Task<ProfileViewModel> getInstance(FactoryServices factoryServices) {
            if(profileViewModel == null) {
                profileViewModel = new ProfileViewModel(factoryServices.CustomerServiceInstance());
            }

            return profileViewModel;
        }

    }
}
