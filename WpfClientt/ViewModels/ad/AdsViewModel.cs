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
using WpfClientt.services.filtering;
using AsyncAwaitBestPractices.MVVM;

namespace WpfClientt.viewModels {
    public class AdsViewModel : BaseViewModel, IViewModel {
        private bool enabled = false;

        public ObservableCollection<AdViewModel> Ads { get; } = new ObservableCollection<AdViewModel>();
        private IScroller<Ad> scroller;
        private IAdService adService;

        public ICommand NextPageCommand { get; private set; }
        public ICommand PreviousPageCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public bool Enabled {
            get => enabled;
            private set {
                enabled = value;
                OnPropertyChanged(nameof(Enabled));
            }
        }
        public FilterViewModel FilterViewModel { get; set; }

        private AdsViewModel(IScroller<Ad> scroller,IAdService adService,FilterViewModel filterViewModel) {
            this.scroller = scroller;
            AddCurrentPageAds(scroller);
            this.adService = adService;
            FilterViewModel = filterViewModel;
            NextPageCommand = new AsyncCommand(OnMoveNext);
            PreviousPageCommand = new AsyncCommand(OnMoveBack);
            RefreshCommand = new AsyncCommand(OnReset);
            SearchCommand = new AsyncCommand(OnSearch);
        }

        public static async Task<AdsViewModel> GetInstanceWithSubcategoryAds(FactoryServices factory,Subcategory subcategory) {
            IAdService adService = await factory.AdServiceInstance();
            IScroller<Ad> subcategoryAds = adService.SubcategoryAds(subcategory);
            await subcategoryAds.Init();
            FilterViewModel filterViewModel = await FilterViewModel.GetInstance(factory,subcategory);

            return new AdsViewModel(subcategoryAds, adService, filterViewModel);
        }

        private async Task OnMoveNext() {
            if (await scroller.MoveNext()) {
                AddCurrentPageAds(scroller);
            }
        }

        private async Task OnMoveBack() {
            if (await scroller.MoveBack()) {
                AddCurrentPageAds(scroller);
            }
        }


        private async Task OnSearch() {
            Enabled = false;
            scroller = adService.Fiter(FilterViewModel.GetFilterBuilder());
            await scroller.Init();
            AddCurrentPageAds(scroller);
        }

        private async Task OnReset() {
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
                Ads.Add(new AdViewModel(ad));
            }
        }


    }
}
