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

        public AdDetailsViewModel(FactoryServices factory, long id) {
            adService = factory.AdServiceInstance();
            DisplayedAd = adService.ReadById(id).Result;
        }

    }
}
