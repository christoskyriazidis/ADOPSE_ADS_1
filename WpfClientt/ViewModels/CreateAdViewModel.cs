using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class CreateAdViewModel : IViewModel {
        private static CreateAdViewModel viewModel;

        private IAdService adService;

        private CreateAdViewModel(IAdService adService) {
            this.adService = adService;
        }

        public static async Task<CreateAdViewModel> GetInstance(FactoryServices factory) {
            if (viewModel == null) {
                viewModel = new CreateAdViewModel(factory.AdServiceInstance());
            }

            return viewModel;
        }


    }
}
