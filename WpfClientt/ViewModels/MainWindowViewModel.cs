using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;
using WpfClientt.services;
using WpfClientt.viewModels.menu;

namespace WpfClientt.viewModels {
    public class MainWindowViewModel : BaseViewModel {

        private IViewModel currentPageView;
        private IMenu currentMenuView;
        private FactoryServices factory;
        private Stack<IViewModel> history = new Stack<IViewModel>();
        public bool IsBackButtonVisible { 
            get {
                return history.Count > 1;
            }
        }

        public IViewModel CurrentPageViewModel {
            get {
                return currentPageView;
            }
            set {
                currentPageView = value;
                OnPropertyChanged("CurrentPageViewModel");
            }
        }

        public IMenu CurrentMenuView {
            get {
                return currentMenuView;
            }
            set {
                currentMenuView = value;
                OnPropertyChanged("CurrentMenuView");
            }
        }

        public MainWindowViewModel() {
            factory = new FactoryServices();
            CurrentMenuView = new GuestMenu();
            ChangeToDisplayView("Welcome To Easy Market!");

            Mediator.Subscribe("AdsView", ChangeToAdsView);
            Mediator.Subscribe("AdsSubcategoryView", ChangeToAdsSubcategoryView);
            Mediator.Subscribe("CategoriesView", ChangeToCategoriesView);
            Mediator.Subscribe("SubcategoriesView", ChangeToSubcategoriesView);
            Mediator.Subscribe("RegisterView", ChangeToRegisterView);
            Mediator.Subscribe("LoginView", ChangeToLoginView);
            Mediator.Subscribe("AdDetailsView", ChangeToAdDetailsView);
            Mediator.Subscribe("CreateView", ChangeToCreateAdView);
            Mediator.Subscribe("ProfileView", ChangeToProfileView);
            Mediator.Subscribe("BackView", PreviousViewModel);
        }

        private async void ChangeToAdsSubcategoryView(object subcategory) {
            ChangeToDisplayView("Loading Page");
            AddToHistory(await AdsViewModel.GetInstanceWithSubcategoryAds(factory,(Subcategory)subcategory));
        }

        private void ChangeViewModel(IViewModel viewModel) {
            CurrentPageViewModel = viewModel;
        }

        private async void ChangeToSubcategoriesView(object category) {
            ChangeToDisplayView("Loading Page");
            AddToHistory( await SubcategoriesViewModel.GetInstance(factory, (Category)category) );
        }

        private async void ChangeToAdsView(object obj) {
            ChangeToDisplayView("Loading Page");
            AddToHistory(await AdsViewModel.GetInstanceWithAllAds(factory));
        }

        private async void ChangeToRegisterView(object obj) {
            ChangeToDisplayView("Loading Page");
            AddToHistory(await RegisterViewModel.GetInstance(factory));
        }

        private async void ChangeToLoginView(object obj) { 
        
        }

        private async void ChangeToCategoriesView(object obj) {
            ChangeToDisplayView("Loading Page");
            AddToHistory(await CategoriesViewModel.GetInstance(factory));
        }

        private async void ChangeToCreateAdView(object obj) {
            ChangeToDisplayView("Loading Page");
            AddToHistory(await CreateAdViewModel.GetInstance(factory));
        }

        private void ChangeToDisplayView(string text) {
            CurrentPageViewModel = DisplayTextViewModel.GetInstance(text);
        }

        private async void ChangeToAdDetailsView(object param) {
            ChangeToDisplayView("Loading Page");
            AddToHistory(await AdDetailsViewModel.GetInstance((Ad)param));
        }

        private async void ChangeToProfileView(object param) {
            ChangeToDisplayView("Loading Page");
            ChangeViewModel(await ProfileViewModel.getInstance(factory));
        }

        private IViewModel currentViewModel() {
            return history.Peek();
        }

        private void PreviousViewModel(object param) {
            history.Pop();
            ChangeViewModel(currentViewModel());
            OnPropertyChanged("IsBackButtonVisible");
        }

        private void AddToHistory(IViewModel viewModel) {
            if (!history.Contains(viewModel)) {
                history.Push(viewModel);
            }
            ChangeViewModel(viewModel);
            OnPropertyChanged("IsBackButtonVisible");
        }
    }
}
