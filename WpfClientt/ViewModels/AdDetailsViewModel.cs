using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public class AdDetailsViewModel : BaseViewModel,IViewModel {
        private static AdDetailsViewModel instance;

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

        private AdDetailsViewModel() {
            DisplayedAd = ad;
        }

        public async static Task<AdDetailsViewModel> GetInstance(FactoryServices factory, long id) {
            if (instance == null) {
                instance = new AdDetailsViewModel();
            }
            Ad ad = await factory.AdServiceInstance().ReadById(id);
            instance.DisplayedAd = ad;
            return instance;
        }

    }
}
