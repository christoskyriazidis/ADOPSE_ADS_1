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
using WpfClientt.services.filtering;

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

        private AdsViewModel(IScroller<Ad> scroller,IAdService adService,FilterViewModel filterViewModel) {
            this.scroller = scroller;
            AddCurrentPageAds(scroller);
            this.adService = adService;
            FilterViewModel = filterViewModel;
            NextPageCommand = new DelegateCommand(OnMoveNext);
            PreviousPageCommand = new DelegateCommand(OnMoveBack);
            ReadMoreCommand = new DelegateCommand(OnReadMore);
            SearchCommand = new DelegateCommand(OnSearch);
            ResetCommand = new DelegateCommand(OnReset);
        }

        public static async Task<AdsViewModel> GetInstanceWithSubcategoryAds(FactoryServices factory,Subcategory subcategory) {
            IAdService adService = await factory.AdServiceInstance();
            IScroller<Ad> subcategoryAds = adService.SubcategoryAds(subcategory);
            await subcategoryAds.Init();
            FilterViewModel filterViewModel = await FilterViewModel.GetInstance(factory,subcategory);

            return new AdsViewModel(subcategoryAds, adService, filterViewModel);
        }

        private async void OnMoveNext(object param) {
            if (await scroller.MoveNext()) {
                AddCurrentPageAds(scroller);
            }
        }

        private async void OnMoveBack(object param) {
            if (await scroller.MoveBack()) {
                AddCurrentPageAds(scroller);
            }
        }

        private void OnReadMore(object param) {
            Mediator.Notify("AdDetailsView", param);
        }

        private async void OnSearch(object param) {
            Enabled = false;
            scroller = adService.Fiter(FilterViewModel.GetFilterBuilder());
            await scroller.Init();
            AddCurrentPageAds(scroller);
        }

        private async void OnReset(object param) {
            Enabled = false;
            FilterViewModel.Reset();
            scroller = adService.Scroller();
            await scroller.Init();
            AddCurrentPageAds(scroller);
        }

        private void AddCurrentPageAds(IScroller<Ad> scroller) {
            Enabled = true;
            Ads.Clear();
            foreach (Ad ad in scroller.CurrentPage().Objects()) {
                Ads.Add(ad);
            }
        }


    }
}
