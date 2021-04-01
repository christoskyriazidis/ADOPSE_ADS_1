using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class AdDetailsViewModel : BaseViewModel,IViewModel {

        private IAdService adService;
        private Ad ad;

        public Ad DisplayedAd {
            get {
                return ad;
            }
            private set {
                ad = value;
                OnPropertyChanged("DisplayedAd");
            } 
        }

        private AdDetailsViewModel(FactoryServices factory, Ad ad) {
            adService = factory.AdServiceInstance();
            DisplayedAd = ad;
        }

        public async static Task<AdDetailsViewModel> GetInstance(FactoryServices factory, long id) {
            Ad ad = await factory.AdServiceInstance().ReadById(id);
            return new AdDetailsViewModel(factory, ad);
        }

    }
}
