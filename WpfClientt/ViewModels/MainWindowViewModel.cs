using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;
using WpfClientt.services;

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
                OnPropertyChanged(nameof(CurrentPageViewModel));
            }
        }

        public IMenu CurrentMenuView {
            get {
                return currentMenuView;
            }
            set {
                currentMenuView = value;
                OnPropertyChanged(nameof(CurrentMenuView));
            }
        }

        public MainWindowViewModel() {
            factory = new FactoryServices();
            CurrentMenuView = new GuestMenu();
            ChangeToDisplayView("Welcome To Easy Market!");

            Mediator.Subscribe("DisplayPageView", ChangeToDisplayView);
            Mediator.Subscribe("ChangeToLoginMenuView", ChangeToLoginMenuView);
            Mediator.Subscribe("AdsSubcategoryView", ChangeToAdsSubcategoryView);
            Mediator.Subscribe("CategoriesView", ChangeToCategoriesView);
            Mediator.Subscribe("SubcategoriesView", ChangeToSubcategoriesView);
            Mediator.Subscribe("RegisterView", ChangeToRegisterView);
            Mediator.Subscribe("LoginView", ChangeToLoginView);
            Mediator.Subscribe("AdDetailsView", ChangeToAdGuestDetailsView);
            Mediator.Subscribe("CreateAdView", ChangeToCreateAdView);
            Mediator.Subscribe("ProfileView", ChangeToProfileView);
            Mediator.Subscribe("BackView", PreviousViewModel);
            Mediator.Subscribe("ChatsView", ChangeToChatsViewModel);
            Mediator.Subscribe("NotificationsView", ChangeToNotificationsView);
        }


        private Task ChangeViewModel(IViewModel viewModel) {
            CurrentPageViewModel = viewModel;
            return Task.CompletedTask;
        }

        private Task ChangeMenuView(IMenu menu) {
            if(menu is LoginCustomerMenu) {
                Mediator.Unsubscribe("AdDetailsView", ChangeToAdGuestDetailsView);
                Mediator.Subscribe("AdDetailsView", ChangeToAdDetailsView);
            }
            this.CurrentMenuView = menu;
            return Task.CompletedTask;
        }

        private async Task ChangeToLoginMenuView(object param) {
            await ChangeMenuView(await LoginCustomerMenu.GetInstance(factory));
        }
        private async Task ChangeToNotificationsView(object _) {
            await ChangeToDisplayView("Loading Page");
            AddToHistory(await NotificationsViewModel.GetInstance(factory));
        }

        private async Task ChangeToAdsSubcategoryView(object subcategory) {
            await ChangeToDisplayView("Loading Page");
            AddToHistory(await AdsViewModel.GetInstanceWithSubcategoryAds(factory,(Subcategory)subcategory));
        }

        private async Task ChangeToSubcategoriesView(object category) {
            await ChangeToDisplayView("Loading Page");
            AddToHistory( await SubcategoriesViewModel.GetInstance(factory, (Category)category) );
        }

        private async Task ChangeToRegisterView(object obj) {
            await ChangeToDisplayView("Loading Page");
            AddToHistory(await RegisterViewModel.GetInstance(factory));
        }

        private async Task ChangeToLoginView(object obj) {
            AddToHistory(await LoginViewModel.GetInstance(factory));
        }

        private async Task ChangeToCategoriesView(object obj) {
            await ChangeToDisplayView("Loading Page");
            AddToHistory(await CategoriesViewModel.GetInstance(factory));
        }

        private async Task ChangeToCreateAdView(object obj) {
            await ChangeToDisplayView("Loading Page");
            AddToHistory(await CreateAdViewModel.GetInstance(factory));
        }

        private Task ChangeToDisplayView(object text) {
            CurrentPageViewModel = DisplayTextViewModel.GetInstance((string)text);
            return Task.CompletedTask;
        }

        private async Task ChangeToAdGuestDetailsView(object param) {
            await ChangeToDisplayView("Loading Page");
            AddToHistory(await AdGuestDetailsViewModel.GetInstance((Ad)param,factory));
        }

        private async Task ChangeToAdDetailsView(object param) {
            await ChangeToDisplayView("Loading Page");
            AddToHistory(await AdDetailsViewModel.GetInstance((Ad)param, factory));
        }

        private async Task ChangeToProfileView(object param) {
            await ChangeToDisplayView("Loading Page");
            await ChangeViewModel(await ProfileViewModel.GetInstance(factory));
        }

        private async Task ChangeToChatsViewModel(object obj) {
            AddToHistory(await ChatsViewModel.GetInstance(factory));
        }
        private IViewModel currentViewModel() {
            return history.Peek();
        }

        private async Task PreviousViewModel(object param) {
            history.Pop();
            await ChangeViewModel(currentViewModel());
            OnPropertyChanged(nameof(IsBackButtonVisible));
        }

        private async void AddToHistory(IViewModel viewModel) {
            if (!history.Contains(viewModel)) {
                history.Push(viewModel);
            }
            await ChangeViewModel(viewModel);
            OnPropertyChanged(nameof(IsBackButtonVisible));
        }
    }
}
