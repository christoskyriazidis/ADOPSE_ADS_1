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
using WpfClientt.viewModels.filters;

namespace WpfClientt.viewModels {
    public class AdsViewModel : BaseViewModel, IViewModel {
        private bool enabled = false;

        public ObservableCollection<Ad> Ads { get; } = new ObservableCollection<Ad>();
        private IScroller<Ad> scroller;
        private IAdService adService;

        public ICommand NextPageCommand { get; private set; }
        public ICommand PreviousPageCommand { get; private set; }
        public ICommand ReadMoreCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public bool Enabled {
            get => enabled;
            private set {
                enabled = value;
                OnPropertyChanged("Enabled");
            }
        }
        public FilterViewModel FilterViewModel { get; set; }

        public AdsViewModel(FactoryServices factory) {
            adService = factory.AdServiceInstance();
            scroller = this.adService.Scroller();
            scroller.Init(AddCurrentPageAds);
            FilterViewModel = new FilterViewModel(factory);
            NextPageCommand = new DelegateCommand(OnMoveNext);
            PreviousPageCommand = new DelegateCommand(OnMoveBack);
            ReadMoreCommand = new DelegateCommand(OnReadMore);
            SearchCommand = new DelegateCommand(OnSearch);
            ResetCommand = new DelegateCommand(OnReset);
        }

        private async void OnMoveNext(object param) {
            if (await scroller.MoveNext()) {
                Ads.Clear();
                foreach (Ad ad in scroller.CurrentPage().Objects()) {
                    Ads.Add(ad);
                }
            }
        }

        private async void OnMoveBack(object param) {
            if (await scroller.MoveBack()) {
                Ads.Clear();
                foreach (Ad ad in scroller.CurrentPage().Objects()) {
                    Ads.Add(ad);
                }
            }
        }

        private void OnReadMore(object param) {
            Mediator.Notify("AdDetailsView", param ?? throw new ArgumentNullException("The id should not be null"));
        }

        private void OnSearch(object param) {
            Enabled = false;
            Ads.Clear();
            scroller = adService.Fiter(FilterViewModel.GetFilterBuilder());
            scroller.Init(AddCurrentPageAds);
        }

        private void OnReset(object param) {
            Enabled = false;
            FilterViewModel.Reset();
            Ads.Clear();
            scroller = adService.Scroller();
            scroller.Init(AddCurrentPageAds);
        }

        private void AddCurrentPageAds(IScroller<Ad> scroller) {
            Enabled = true;
            foreach (Ad ad in scroller.CurrentPage().Objects()) {
                Ads.Add(ad);
            }
        }


    }
}
