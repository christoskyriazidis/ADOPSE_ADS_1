using WpfClientt.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.services;
using System.Windows.Input;
using System.Diagnostics;

namespace WpfClientt.viewModels {
    public class AdsViewModel : BaseViewModel,IViewModel {

        public ObservableCollection<Ad> ads { get; } = new ObservableCollection<Ad>();
        private IScroller<Ad> scroller;
        private IAdService adService;
        public ICommand NextPageCommand { get; private set; }
        public ICommand PreviousPageCommand { get; private set; }

        public AdsViewModel(IAdService adService) {
            this.adService = adService;
            this.scroller = this.adService.Scroller();
            this.scroller.Init(scrll => {
                foreach (Ad ad in scrll.CurrentPage().Objects()) {
                    ads.Add(ad);
                }
            });

            NextPageCommand = new DelegateCommand(OnMoveNext);
            PreviousPageCommand = new DelegateCommand(OnMoveBack);
        }

        private async void OnMoveNext(object param) {
            if (await scroller.MoveNext()) {
                ads.Clear();
                foreach (Ad ad in scroller.CurrentPage().Objects()) {
                    ads.Add(ad);
                }
            }
        }

        private async void OnMoveBack(object param) {
            if (await scroller.MoveBack()) {
                ads.Clear();
                foreach (Ad ad in scroller.CurrentPage().Objects()) {
                    ads.Add(ad);
                }
            }
        }
     

    }
}
