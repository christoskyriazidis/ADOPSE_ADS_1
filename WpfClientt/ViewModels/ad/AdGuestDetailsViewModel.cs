using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class AdGuestDetailsViewModel : BaseViewModel,IViewModel {
    
        public Ad Ad { get; set; }

        public Customer Seller { get; set; }

        private AdGuestDetailsViewModel(Ad ad,Customer seller) {
            this.Ad = ad;
            this.Seller = seller;
        }

        public static async Task<AdGuestDetailsViewModel> GetInstance(Ad ad,FactoryServices factory) {
            ICustomerService customerService = factory.CustomerServiceInstance();
            Customer seller = await customerService.ReadById(ad.CustomerId);

            return new AdGuestDetailsViewModel(ad, seller);
        }

    }
}
